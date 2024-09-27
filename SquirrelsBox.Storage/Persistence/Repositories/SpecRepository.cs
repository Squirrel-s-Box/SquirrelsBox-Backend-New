using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;
using System.Data;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class SpecRepository : BaseRepository<AppDbContext>, IGenericRepositoryWithMassive<Spec>, IGenericReadRepository<Spec>
    {
        public SpecRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddMassiveAsync(ICollection<Spec> modelList)
        {
            await _context.PersonalizedSpecs.AddRangeAsync(modelList);
            await _context.SaveChangesAsync();

            foreach (var model in modelList)
            {
                await _context.Entry(model).ReloadAsync();
            }
        }

        public async Task DeleteteMassiveAsync(ICollection<int> ids, string userCode)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Fetch existing specifications to delete
                var existingEntities = await _context.PersonalizedSpecs
                    .Where(spec => ids.Contains(spec.Id))
                    .ToListAsync();

                // Prepare deletion data for logging
                var logDeletionData = new DataTable();
                logDeletionData.Columns.Add("SectionId", typeof(int));
                logDeletionData.Columns.Add("ItemId", typeof(int));
                logDeletionData.Columns.Add("SpecId", typeof(int));

                var relatedData = await _context.BoxesSectionsList
                    .Where(bsr => bsr.Section.SectionItemsList
                        .Any(sir => sir.Item.Specs
                            .Any(spec => ids.Contains(spec.Id))))
                    .Select(bsr => new
                    {
                        BoxId = bsr.BoxId,
                        SectionId = bsr.SectionId,
                        ItemId = bsr.Section.SectionItemsList
                            .Where(sir => sir.Item.Specs
                                .Any(spec => ids.Contains(spec.Id)))
                            .Select(sir => sir.ItemId)
                            .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();

                // Populate the DataTable with SpecIds
                foreach (var specId in ids)
                {
                    logDeletionData.Rows.Add(relatedData.SectionId, relatedData.ItemId, specId);
                }

                // Create parameter for logging procedure
                var deletionDataParam = new SqlParameter("@DeletionData", logDeletionData)
                {
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "dbo.LogBoxDeletionType"
                };

                // Call the logging stored procedure
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_LogBoxDeletion @UserCode = {0}, @BoxId = {1}, @DeletionData = @DeletionData, @LogBoxDeletion = 0;",
                    userCode, relatedData.BoxId, deletionDataParam // Pass the actual BoxId here
                );

                // Remove existing specifications
                _context.PersonalizedSpecs.RemoveRange(existingEntities);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception for debugging purposes (implement your logging mechanism here)
                throw new InvalidOperationException("An error occurred while deleting the specifications.", ex);
            }
        }

        public async Task<IEnumerable<Spec>> ListAllByIdAsync(int id)
        {
            return await _context.PersonalizedSpecs
                .Where(b => b.ItemId == id)
                .ToListAsync();
        }

        public Task<IEnumerable<Spec>> ListAllByUserCodeAsync(string userCode)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateMassiveAsync(ICollection<Spec> modelList)
        {
            var ids = modelList.Select(spec => spec.Id).ToList();

            var existingEntities = await _context.PersonalizedSpecs
                                                    .Where(spec => ids.Contains(spec.Id))
                                                    .ToListAsync();

            foreach (var existingEntity in existingEntities)
            {
                // Find the matching model from the input list
                var matchingModel = modelList.FirstOrDefault(spec => spec.Id == existingEntity.Id);
                if (matchingModel != null)
                {
                    // Update the properties of the existing entity
                    existingEntity.HeaderName = matchingModel.HeaderName;
                    existingEntity.Value = matchingModel.Value;
                    existingEntity.ValueType = matchingModel.ValueType;
                    existingEntity.Active = matchingModel.Active;
                    existingEntity.LastUpdateDate = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();

            // Reload the updated entities from the database if necessary
            foreach (var existingEntity in existingEntities)
            {
                await _context.Entry(existingEntity).ReloadAsync();
            }
        }

    }
}
