using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class SectionItemRelationshipRepository : BaseRepository<AppDbContext>, IGenericRepository<SectionItemRelationship>, IGenericReadRepository<SectionItemRelationship>
    {
        public SectionItemRelationshipRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(SectionItemRelationship model)
        {
            var ItemCreated = await _context.Items.AddAsync(model.Item);
            await _context.SaveChangesAsync();
            model.ItemId = ItemCreated.Entity.Id;
            await _context.SectionsItemsList.AddAsync(model);
        }

        public async Task DeleteAsync(SectionItemRelationship model)
        {
            //_context.SectionsItemsList.Remove(model);
            _context.Items.Remove(model.Item);
             await _context.SaveChangesAsync();
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
                await _context.UpdateSectionItemRelationship(
                model.SectionId,
                    model.ItemId,
                    // NewValues
                    model.Item.Id
                );
            }
            else if (model.SectionId == 0)
            {
                _context.Items.Update(model.Item);
            }
        }
    }
}
