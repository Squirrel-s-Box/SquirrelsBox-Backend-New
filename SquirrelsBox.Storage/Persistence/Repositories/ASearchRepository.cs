using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Interfaces;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class ASearchRepository : BaseRepository<AppDbContext>, IGenericSearchRepository, IActionLogRepository
    {
        public ASearchRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<object> CounterByUserCodeAsync(string userCode)
        {
            var result = await (from b in _context.Boxes
                                where b.UserCodeOwner == userCode
                                join bs in _context.BoxesSectionsList on b.Id equals bs.BoxId
                                join si in _context.SectionsItemsList on bs.SectionId equals si.SectionId
                                group new { b, bs, si } by new { b.Id, b.UserCodeOwner } into g
                                select new
                                {
                                    NumberOfBoxes = g.Select(x => x.b).Distinct().Count(),
                                    NumberOfSections = g.Select(x => x.bs.SectionId).Distinct().Count(),
                                    NumberOfItems = g.Select(x => x.si.ItemId).Distinct().Count()
                                })
                                .FirstOrDefaultAsync();

            return result ?? new { NumberOfBoxes = 0, NumberOfSections = 0, NumberOfItems = 0 };
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

        public async Task<IEnumerable<ActionLog>> ReadActionLog(string userCode)
        {
            return await _context.ActionLog.Where(x => x.Usercode == userCode).ToListAsync();
        }
    }
}
