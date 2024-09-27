using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class BoxRepository : BaseRepository<AppDbContext>, IGenericRepositoryWithCascade<Box>, IGenericReadRepository<Box>
    {
        public BoxRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Box model)
        {
            await _context.Boxes.AddAsync(model);
            _context.Entry(model).GetDatabaseValues();
        }

        public async Task DeleteAsync(Box model)
        {
            _context.Boxes.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCascadeAsync(Box model, string userCode, bool cascade)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var boxId = model.Id;

                if (cascade)
                {
                    // Get related sections, their items, and specs
                    var relatedData = await _context.BoxesSectionsList
                        .Where(bsr => bsr.BoxId == boxId)
                        .Select(bsr => new
                        {
                            SectionId = bsr.Section.Id,
                            SectionItems = bsr.Section.SectionItemsList.Select(sir => new
                            {
                                ItemId = sir.Item.Id,
                                Specs = sir.Item.Specs.Select(spec => spec.Id).ToList() // Get the IDs of specs related to the item
                            }).ToList()
                        })
                        .ToListAsync();

                    var sectionIds = relatedData.Select(rd => rd.SectionId).Distinct().ToList();
                    var itemIds = relatedData.SelectMany(rd => rd.SectionItems).Select(si => si.ItemId).Distinct().ToList();
                    var specIds = relatedData.SelectMany(rd => rd.SectionItems.SelectMany(si => si.Specs)).Distinct().ToList();

                    // Prepare data for logging deletions
                    var logDeletionData = new DataTable();
                    logDeletionData.Columns.Add("SectionId", typeof(int));
                    logDeletionData.Columns.Add("ItemId", typeof(int));
                    logDeletionData.Columns.Add("SpecId", typeof(int));

                    // Log data for each section and its items/specifications
                    foreach (var sectionData in relatedData)
                    {
                        var sectionId = sectionData.SectionId;

                        // Log section
                        logDeletionData.Rows.Add(sectionId, DBNull.Value, DBNull.Value);

                        foreach (var itemData in sectionData.SectionItems)
                        {
                            var itemId = itemData.ItemId;

                            // Log item
                            logDeletionData.Rows.Add(sectionId, itemId, DBNull.Value);

                            // Log specifications for this item
                            foreach (var specId in itemData.Specs)
                            {
                                logDeletionData.Rows.Add(sectionId, itemId, specId); // Log sectionId, itemId, and specId
                            }
                        }
                    }

                    var deletionDataParam = new SqlParameter("@DeletionData", logDeletionData)
                    {
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "dbo.LogBoxDeletionType"
                    };

                    // Call the logging stored procedure
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC sp_LogBoxDeletion @UserCode = {0}, @BoxId = {1}, @DeletionData = @DeletionData, @LogBoxDeletion = 1;",
                        userCode, boxId, deletionDataParam
                    );

                    // Delete from sections_items_list, items, and sections
                    if (itemIds.Any())
                    {
                        await _context.Database.ExecuteSqlRawAsync($"DELETE FROM sections_items_list WHERE item_id IN ({string.Join(",", itemIds)});");
                        await _context.Database.ExecuteSqlRawAsync($"DELETE FROM items WHERE Id IN ({string.Join(",", itemIds)});");
                    }
                    if (sectionIds.Any())
                    {
                        await _context.Database.ExecuteSqlRawAsync($"DELETE FROM boxes_sections_list WHERE section_id IN ({string.Join(",", sectionIds)});");
                        await _context.Database.ExecuteSqlRawAsync($"DELETE FROM sections WHERE Id IN ({string.Join(",", sectionIds)});");
                    }
                }

                // Delete shared boxes bonds and permissions
                var sharedBoxes = await _context.SharedBoxes.Where(sb => sb.BoxId == boxId).ToListAsync();
                if (sharedBoxes.Any())
                {
                    _context.SharedBoxes.RemoveRange(sharedBoxes);
                    await _context.SaveChangesAsync();
                }

                // Delete the box itself
                await DeleteAsync(model);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        public async Task<Box> FindByCodeAsync(string value)
        {
            return await _context.Boxes.FirstOrDefaultAsync(i => i.UserCodeOwner == value);
        }

        public async Task<Box> FindByIdAsync(int id)
        {
            return await _context.Boxes.FindAsync(id);
        }

        public Task<IEnumerable<Box>> ListAllByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Box>> ListAllByUserCodeAsync(string userCode)
        {
            return await _context.Boxes.Where(b => b.UserCodeOwner == userCode).ToListAsync();
        }

        public void Update(Box model)
        {
            _context.Boxes.Update(model);
            _context.Entry(model).GetDatabaseValues();
        }
    }
}
