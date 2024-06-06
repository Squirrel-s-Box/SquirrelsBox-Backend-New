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
            
        public async Task AddAsync(Spec model)
        {
            var SpecCreated = await _context.PersonalizedSpecs.AddAsync(model);
            await _context.SaveChangesAsync();
            //await _context.PersonalizedSpecsItemsList.AddAsync(model);
        }

        public async Task AddMassiveAsync(ICollection<Spec> modelList)
        {
            await _context.PersonalizedSpecs.AddRangeAsync(modelList);
        }

        public async Task DeleteAsync(Spec model)
        {
            //_context.PersonalizedSpecsItemsList.Remove(model);
            _context.PersonalizedSpecs.Remove(model);
            await _context.SaveChangesAsync();
        }

        public Task<Spec> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<Spec> FindByIdAsync(int id)
        {
            // return await _context.PersonalizedSpecsItemsList
            // .Include(b => b.Spec)
            //.FirstOrDefaultAsync(x => x.SpecId == id);
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Spec>> ListAllByIdAsync(int id)
        {
            //return await _context.PersonalizedSpecsItemsList
            //    .Include(b => b.SpecId)
            //    .Where(b => b.ItemId == id)
            //    .ToListAsync();
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Spec>> ListAllByUserCodeAsync(string userCode)
        {
            throw new NotImplementedException();
        }

        public async void Update(Spec model)
        {
            _context.PersonalizedSpecs.Update(model);
        }
    }
}
