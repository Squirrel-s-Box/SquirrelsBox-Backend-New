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

        public void Delete(Box model)
        {
            _context.Boxes.Remove(model);
        }

        public void DeleteCascade(Box model, bool cascade = false)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Delete(model);

                if (cascade)
                {
                    var boxId = model.Id;

                    // Get related sections, items, and personalized specs in a single query
                    var relatedData = _context.BoxesSectionsList
                                              .Where(bsr => bsr.BoxId == boxId)
                                              .Select(bsr => new
                                              {
                                                  SectionId = bsr.SectionId,
                                                  SectionItems = bsr.Section.SectionItemsList
                                                                       .Select(sir => new
                                                                       {
                                                                           ItemId = sir.ItemId,
                                                                           ItemSpecs = sir.Item.ItemSpecsList
                                                                                                .Select(isr => isr.SpecId)
                                                                       })
                                              })
                                              .ToList();

                    // Extract distinct IDs
                    var sectionIds = relatedData.Select(rd => rd.SectionId).Distinct().ToList();
                    var itemIds = relatedData.SelectMany(rd => rd.SectionItems.Select(si => si.ItemId)).Distinct().ToList();
                    var specIds = relatedData.SelectMany(rd => rd.SectionItems.SelectMany(si => si.ItemSpecs)).Distinct().ToList();

                    // Bulk delete using raw SQL for better performance
                    if (sectionIds.Any())
                    {
                        var sectionIdsParam = string.Join(",", sectionIds);
                        _context.Database.ExecuteSqlRawAsync($"DELETE FROM sections WHERE Id IN ({sectionIdsParam})");
                    }
                    if (itemIds.Any())
                    {
                        var itemIdsParam = string.Join(",", itemIds);
                        _context.Database.ExecuteSqlRawAsync($"DELETE FROM items WHERE Id IN ({itemIdsParam})");
                    }
                    if (specIds.Any())
                    {
                        var specIdsParam = string.Join(",", specIds);
                        _context.Database.ExecuteSqlRawAsync($"DELETE FROM personalized_specs WHERE Id IN ({specIdsParam})");
                    }

                    // Optionally, remove shared boxes if needed
                    //var sharedBoxes = _context.SharedBox.Where(sb => sb.BoxId == boxId).ToList();
                    //if (sharedBoxes.Any())
                    //{
                    //    _context.SharedBox.RemoveRange(sharedBoxes);
                    //}
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
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
