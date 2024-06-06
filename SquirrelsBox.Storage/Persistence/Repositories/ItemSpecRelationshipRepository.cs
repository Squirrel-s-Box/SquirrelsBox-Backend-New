using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class ItemSpecRelationshipRepository : BaseRepository<AppDbContext>, IGenericRepository<ItemSpecRelationship>, IGenericReadRepository<ItemSpecRelationship>
    {
        public ItemSpecRelationshipRepository(AppDbContext context) : base(context)
        {
        }
            
        public async Task AddAsync(ItemSpecRelationship model)
        {
            var SpecCreated = await _context.PersonalizedSpecs.AddAsync(model.Spec);
            await _context.SaveChangesAsync();
            model.SpecId = SpecCreated.Entity.Id;
            await _context.PersonalizedSpecsItemsList.AddAsync(model);
        }

        public async Task DeleteAsync(ItemSpecRelationship model)
        {
            _context.PersonalizedSpecsItemsList.Remove(model);
            _context.PersonalizedSpecs.Remove(model.Spec);
            await _context.SaveChangesAsync();
        }

        public Task<ItemSpecRelationship> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemSpecRelationship> FindByIdAsync(int id)
        {
            return await _context.PersonalizedSpecsItemsList
            .Include(b => b.Spec)
           .FirstOrDefaultAsync(x => x.SpecId == id);
        }

        public async Task<IEnumerable<ItemSpecRelationship>> ListAllByIdAsync(int id)
        {
            return await _context.PersonalizedSpecsItemsList
                .Include(b => b.SpecId)
                .Where(b => b.ItemId == id)
                .ToListAsync();
        }

        public Task<IEnumerable<ItemSpecRelationship>> ListAllByUserCodeAsync(string userCode)
        {
            throw new NotImplementedException();
        }

        public async void Update(ItemSpecRelationship model)
        {
            if (model.ItemId != 0)
            {
                await _context.UpdateItemSpecRelationship(
                model.ItemId,
                    model.SpecId,
                    // NewValues
                    model.Spec.Id
                );
            }
            else if (model.ItemId == 0)
            {
                _context.PersonalizedSpecs.Update(model.Spec);
            }
        }
    }
}
