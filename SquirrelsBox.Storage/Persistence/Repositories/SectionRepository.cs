using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;
using System.Data;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class SectionRepository : BaseRepository<AppDbContext>, IGenericRepositoryWithCascade<BoxSectionRelationship>, IGenericReadRepository<BoxSectionRelationship>
    {
        public SectionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(BoxSectionRelationship model)
        {
            try
            {
                // Agregar la sección
                var sectionCreated = await _context.Sections.AddAsync(model.Section);
                await _context.SaveChangesAsync();

                // Asignar el ID de la sección creada al modelo
                model.SectionId = sectionCreated.Entity.Id;

                // Agregar la relación entre la sección y la caja
                await _context.BoxesSectionsList.AddAsync(model);
                await _context.SaveChangesAsync(); // Guardar los cambios después de agregar la relación

                Console.WriteLine("Section and BoxSectionRelationship added successfully.");
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"An error occurred while adding the BoxSectionRelationship: {ex.Message}");
                throw; // Rethrow the exception if you want it to be handled further up the call stack
            }
        }


        public async Task DeleteAsync(BoxSectionRelationship model)
        {
            //_context.BoxesSectionsList.Remove(model);
            _context.Sections.Remove(model.Section);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCascadeAsync(BoxSectionRelationship model, bool cascade)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var sectionId = model.SectionId;

                if (cascade)
                {
                    // Get related items for the section
                    var relatedData = await _context.BoxesSectionsList
                        .Where(bsr => bsr.SectionId == sectionId)
                        .SelectMany(bsr => bsr.Section.SectionItemsList.Select(sir => new
                        {
                            ItemId = sir.Item.Id,
                            Specs = sir.Item.Specs.Select(spec => spec.Id).ToList()
                        }))
                        .ToListAsync();

                    // Extract distinct IDs
                    var itemIds = relatedData.Select(rd => rd.ItemId).Distinct().ToList();
                    var specIds = relatedData.SelectMany(rd => rd.Specs).Distinct().ToList();

                    // Prepare data for logging deletions
                    var logDeletionData = new DataTable();
                    logDeletionData.Columns.Add("SectionId", typeof(int));
                    logDeletionData.Columns.Add("ItemId", typeof(int));
                    logDeletionData.Columns.Add("SpecId", typeof(int));

                    logDeletionData.Rows.Add(sectionId, DBNull.Value, DBNull.Value);
                    foreach (var itemData in relatedData)
                    {
                        var itemId = itemData.ItemId;

                        // Log the section and item ID
                        logDeletionData.Rows.Add(sectionId, itemId, DBNull.Value);

                        // Log specifications for this item
                        foreach (var specId in itemData.Specs)
                        {
                            logDeletionData.Rows.Add(sectionId, itemId, specId);
                        }
                    }

                    var userCode = "Prueba456"; // Replace with actual user code if available
                    var boxId = await _context.Boxes
                        .Where(box => box.BoxSectionsList.Any(bsr => bsr.SectionId == sectionId))
                        .Select(box => box.Id)
                        .FirstOrDefaultAsync();

                    // Create parameter for logging procedure
                    var deletionDataParam = new SqlParameter("@DeletionData", logDeletionData)
                    {
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "dbo.LogBoxDeletionType"
                    };

                    // Call the logging stored procedure
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC sp_LogBoxDeletion @UserCode = {0}, @BoxId = {1}, @DeletionData = @DeletionData, @LogBoxDeletion = 0;",
                        userCode, boxId, deletionDataParam
                    );

                    // Bulk delete items and their specs using raw SQL for better performance
                    if (itemIds.Any())
                    {
                        await _context.Database.ExecuteSqlRawAsync($"DELETE FROM sections_items_list WHERE item_id IN ({string.Join(",", itemIds)});");
                        await _context.Database.ExecuteSqlRawAsync($"DELETE FROM items WHERE Id IN ({string.Join(",", itemIds)});");
                    }

                    // Delete the section from the boxes_sections_list and the section itself
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM boxes_sections_list WHERE section_id = {sectionId};");
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM sections WHERE Id = {sectionId};");
                }

                // Finally, remove the BoxSectionRelationship model
                //_context.BoxesSectionsList.Remove(model);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("An error occurred while deleting the section and its related data.", ex);
            }
        }

        public Task<BoxSectionRelationship> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<BoxSectionRelationship> FindByIdAsync(int id)
        {
            return await _context.BoxesSectionsList
            .Include(b => b.Section)
            .FirstOrDefaultAsync(x => x.SectionId == id);
        }

        public async Task<IEnumerable<BoxSectionRelationship>> ListAllByIdAsync(int id)
        {
            return await _context.BoxesSectionsList
                .Include(b => b.Section)
                .Where(b => b.BoxId == id)
                .ToListAsync();
        }

        public Task<IEnumerable<BoxSectionRelationship>> ListAllByUserCodeAsync(string userCode)
        {
            throw new NotImplementedException();
        }

        public async void Update(BoxSectionRelationship model)
        {
            if (model.BoxId != 0)
            {
                _context.BoxesSectionsList.Update(model);
            }
            else if (model.SectionId != 0)
            {
                _context.Sections.Update(model.Section);
            }
        }

    }
}
