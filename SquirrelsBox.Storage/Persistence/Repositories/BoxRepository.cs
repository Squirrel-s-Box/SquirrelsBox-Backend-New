using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class BoxRepository : BaseRepository<AppDbContext>, IGenericRepositoryWithCascade<Box>, IGenericReadRepository<Box>
    {
        public BoxRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Box model)
        {
            await _context.Boxes.AddAsync(model);
        }

        public async Task DeleteAsync(Box model)
        {
            _context.Boxes.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCascadeAsync(Box model, bool cascade)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var boxId = model.Id;
                if (cascade)
                {
                    var relatedData = await _context.BoxesSectionsList
                        .Where(bsr => bsr.BoxId == boxId)
                        .Select(bsr => new
                        {
                            SectionId = bsr.Section.Id, // Assuming 'Id' is the primary key of the Section entity
                            SectionItems = bsr.Section.SectionItemsList
                                .Select(sir => sir.Item.Id) // Select only the ItemId
                        })
                        .ToListAsync();

                    // Extract distinct IDs
                    var sectionIds = relatedData.Select(rd => rd.SectionId).Distinct().ToList();
                    var itemIds = relatedData.SelectMany(rd => rd.SectionItems).Distinct().ToList();

                    // Bulk delete using raw SQL for better performance
                    if (sectionIds.Any() || itemIds.Any())
                    {
                        var sectionIdsParam = sectionIds.Any() ? string.Join(",", sectionIds) : "NULL";
                        var itemIdsParam = itemIds.Any() ? string.Join(",", itemIds) : "NULL";


                        await _context.Database.ExecuteSqlRawAsync($@"
                            DELETE FROM sections WHERE Id IN ({sectionIdsParam});
                            DELETE FROM items WHERE Id IN ({itemIdsParam});
                        ");
                    }
                }
                //Delete shared boxes bonds and permissions
                var sharedBoxes = await _context.SharedBoxes
                    .Where(sb => sb.BoxId == boxId)
                    .ToListAsync();

                if (sharedBoxes.Any())
                {
                    _context.SharedBoxes.RemoveRange(sharedBoxes);
                    await _context.SaveChangesAsync();
                }

                await DeleteAsync(model);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
