using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class BoxRepository : BaseRepository<AppDbContext>, IGenericRepository<Box>, IGenericReadRepository<Box>
    {
        public BoxRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Box model)
        {
            await _context.Boxes.AddAsync(model);
        }

        public void Delete(Box model)
        {
            _context.Boxes.Remove(model);
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
        }
    }
}
