using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;
using System.Runtime.CompilerServices;

namespace SquirrelsBox.Authentication.Persistence.Repositories
{
    public class AssignedPermissionRepository : BaseRepository<AppDbContext>, IGenericReadRepository<AssignedPermission>, IGenericRepository<AssignedPermission>
    {
        public AssignedPermissionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(AssignedPermission model)
        {
            await _context.AssignedPermissions.AddAsync(model);
        }

        public async Task DeleteAsync(AssignedPermission model)
        {
            _context.AssignedPermissions.Remove(model);
            await _context.SaveChangesAsync();
        }

        public Task<AssignedPermission> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public Task<AssignedPermission> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssignedPermission>> ListAllByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AssignedPermission>> ListAllByUserCodeAsync(string userCode)
        {
            return await _context.AssignedPermissions
                        .Include(ap => ap.Permission) // Include the Permission entity
                        .Where(ap => ap.UserCode == userCode)
                        .Select(ap => new AssignedPermission
                        {
                            Permission = ap.Permission
                        })
                        .ToListAsync();
        }

        public void Update(AssignedPermission model)
        {
            throw new NotImplementedException();
        }
    }
}
