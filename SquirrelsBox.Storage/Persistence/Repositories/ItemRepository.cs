using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;
using System.Data;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class ItemRepository : BaseRepository<AppDbContext>, IGenericRepository<SectionItemRelationship>, IGenericReadRepository<SectionItemRelationship>
    {
        public ItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(SectionItemRelationship model)
        {
            var ItemCreated = await _context.Items.AddAsync(model.Item);
            await _context.SaveChangesAsync();
            model.ItemId = ItemCreated.Entity.Id;
            await _context.SectionsItemsList.AddAsync(model);

            _context.Entry(model).GetDatabaseValues();
        }

        public async Task DeleteAsync(SectionItemRelationship model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if the model is not null
                if (model == null) throw new ArgumentNullException(nameof(model));

                // Prepare deletion data for logging (Section, Item, Specs)
                var logDeletionData = new DataTable();
                logDeletionData.Columns.Add("SectionId", typeof(int));
                logDeletionData.Columns.Add("ItemId", typeof(int));
                logDeletionData.Columns.Add("SpecId", typeof(int));

                // Retrieve the related BoxId
                var boxId = await _context.Boxes
                    .Where(box => box.BoxSectionsList
                        .Any(bsr => bsr.Section.SectionItemsList
                            .Any(sir => sir.Item.Id == model.ItemId)))
                    .Select(box => box.Id)
                    .FirstOrDefaultAsync();

                // Add the SectionId and ItemId to the log
                logDeletionData.Rows.Add(model.SectionId, model.ItemId, DBNull.Value);

                // Get all related Specs to log and delete
                var specIds = await _context.PersonalizedSpecs
                    .Where(spec => spec.ItemId == model.ItemId)
                    .Select(spec => spec.Id)
                    .ToListAsync();

                foreach (var specId in specIds)
                {
                    // Log each spec being deleted
                    logDeletionData.Rows.Add(model.SectionId, model.ItemId, specId);
                }

                // Create SQL parameter for logging procedure
                var deletionDataParam = new SqlParameter("@DeletionData", logDeletionData)
                {
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "dbo.LogBoxDeletionType"
                };

                var userCode = model.Item.UserCodeLog;

                // Call the stored procedure to log the deletion
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_LogBoxDeletion @UserCode = {0}, @BoxId = {1}, @DeletionData = @DeletionData, @LogBoxDeletion = 0;",
                    userCode, boxId, deletionDataParam);

                // Remove the item itself
                _context.Items.Remove(model.Item);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception for debugging purposes (implement your logging mechanism here)
                throw new InvalidOperationException("An error occurred while deleting the item.", ex);
            }
        }


        public Task<SectionItemRelationship> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<SectionItemRelationship> FindByIdAsync(int id)
        {
            return await _context.SectionsItemsList
            .Include(b => b.Item)
           .FirstOrDefaultAsync(x => x.ItemId == id);
        }

        public async Task<IEnumerable<SectionItemRelationship>> ListAllByIdAsync(int id)
        {
            return await _context.SectionsItemsList
                .Include(b => b.Item)
                .Where(b => b.SectionId == id)
                .ToListAsync();
        }

        public Task<IEnumerable<SectionItemRelationship>> ListAllByUserCodeAsync(string userCode)
        {
            throw new NotImplementedException();
        }

        public async void Update(SectionItemRelationship model)
        {
            if (model.SectionId != 0)
            {
                _context.SectionsItemsList.Update(model);
            }
            else if (model.SectionId != 0)
            {
                _context.Items.Update(model.Item);
            }

            _context.Entry(model).GetDatabaseValues();
        }
    }
}
