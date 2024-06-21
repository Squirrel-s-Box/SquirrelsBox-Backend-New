using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class ASearchRepository : BaseRepository<AppDbContext>, IGenericSearchRepository
    {
        public ASearchRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<object> ListFinderAsync(string text, int type)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Search text cannot be null or empty", nameof(text));
            }

            switch (type)
            {
                case 1:
                    return await _context.Boxes
                        .Where(b => b.Name.Contains(text))
                        .ToListAsync();
                case 2:
                    return await _context.Sections
                        .Where(b => b.Name.Contains(text))
                        .ToListAsync();
                case 3:
                    return await _context.Items
                        .Where(b => b.Name.Contains(text))
                        .ToListAsync();
                default:
                    throw new ArgumentException("Invalid type provided", nameof(type));
            }
        }

    }
}
