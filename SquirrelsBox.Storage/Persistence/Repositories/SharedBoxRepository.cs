using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class SharedBoxRepository : BaseRepository<AppDbContext>, IGenericRepository<SharedBox>, IGenericReadRepository<SharedBox>
    {
        public SharedBoxRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(SharedBox model)
        {
            await _context.SharedBoxes.AddAsync(model);
        }

        public async Task DeleteAsync(SharedBox model)
        {
            _context.SharedBoxes.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<SharedBox> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<SharedBox> FindByIdAsync(int id)
        {
            return await _context.SharedBoxes.FindAsync(id);
        }

        public Task<IEnumerable<SharedBox>> ListAllByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SharedBox>> ListAllByUserCodeAsync(string userCode)
        {
            var sharedBoxes = await _context.SharedBoxes
                .Include(sb => sb.Box)
                .Where(sb => sb.UserCodeGuest == userCode && sb.State)
                .ToListAsync();

            return sharedBoxes;
        }



        public void Update(SharedBox model)
        {
            _context.SharedBoxes.Update(model);
        }
    }
}
