﻿using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Persistence.Repositories
{
    public class BoxSectionRelationshipRepository : BaseRepository<AppDbContext>, IGenericRepository<BoxSectionRelationship>, IGenericReadRepository<BoxSectionRelationship>
    {
        public BoxSectionRelationshipRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(BoxSectionRelationship model)
        {
            try
            {
                // Agregar la sección
                var sectionCreated = await _context.Sections.AddAsync(model.Section);
                await _context.SaveChangesAsync();

                // Asignar el ID de la sección creada al modelo
                model.SectionId = sectionCreated.Entity.Id;

                // Agregar la relación entre la sección y la caja
                await _context.BoxesSectionsList.AddAsync(model);
                await _context.SaveChangesAsync(); // Guardar los cambios después de agregar la relación

                Console.WriteLine("Section and BoxSectionRelationship added successfully.");
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"An error occurred while adding the BoxSectionRelationship: {ex.Message}");
                throw; // Rethrow the exception if you want it to be handled further up the call stack
            }
        }


        public async Task DeleteAsync(BoxSectionRelationship model)
        {
            _context.BoxesSectionsList.Remove(model);
            _context.Sections.Remove(model.Section);
            await _context.SaveChangesAsync();
        }

        public Task<BoxSectionRelationship> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<BoxSectionRelationship> FindByIdAsync(int id)
        {
            return await _context.BoxesSectionsList
            .Include(b => b.Section)
            .FirstOrDefaultAsync(x => x.SectionId == id);
        }

        public async Task<IEnumerable<BoxSectionRelationship>> ListAllByIdAsync(int id)
        {
            return await _context.BoxesSectionsList
                .Include(b => b.Section)
                .Where(b => b.BoxId == id)
                .ToListAsync();
        }

        public Task<IEnumerable<BoxSectionRelationship>> ListAllByUserCodeAsync(string userCode)
        {
            throw new NotImplementedException();
        }

        public async void Update(BoxSectionRelationship model)
        {
            if (model.BoxId != 0)
            {
                await _context.UpdateBoxSectionRelationship(
                    model.BoxId,
                    model.SectionId,
                // NewValues
                    model.Section.Id
                );
            }
            else if (model.SectionId == 0)
            {
                _context.Sections.Update(model.Section);
            }
        }
    }
}
