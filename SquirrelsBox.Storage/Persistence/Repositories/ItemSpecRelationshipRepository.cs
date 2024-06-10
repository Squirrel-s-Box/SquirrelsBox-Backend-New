using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class ItemSpecRelationshipRepository : BaseRepository<AppDbContext>, IGenericRepositoryWithMassive<Spec>, IGenericReadRepository<Spec>
    {
        public ItemSpecRelationshipRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddMassiveAsync(ICollection<Spec> modelList)
        {
            await _context.PersonalizedSpecs.AddRangeAsync(modelList);
        }

        public async Task DeleteteMassiveAsync(ICollection<int> ids)
        {
            var existingEntities = await _context.PersonalizedSpecs
                                                        .Where(spec => ids.Contains(spec.Id))
                                                        .ToListAsync();

            _context.PersonalizedSpecs.RemoveRange(existingEntities);

            await _context.SaveChangesAsync();
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
                var matchingModel = modelList.FirstOrDefault(spec => spec.Id == existingEntity.Id);
                if (matchingModel != null)
                {
                    existingEntity.HeaderName = matchingModel.HeaderName;
                    existingEntity.Value = matchingModel.Value;
                    existingEntity.ValueType = matchingModel.ValueType;
                    existingEntity.Active = matchingModel.Active;
                    existingEntity.LastUpdateDate = DateTime.UtcNow;
                }
            }

            _context.PersonalizedSpecs.UpdateRange(existingEntities);

            await _context.SaveChangesAsync();
        }

    }
}
