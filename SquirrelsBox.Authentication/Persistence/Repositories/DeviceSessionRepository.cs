using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Authentication.Domain.Interfaces;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;

namespace SquirrelsBox.Authentication.Persistence.Repositories
{
    public class DeviceSessionRepository : BaseRepository<AppDbContext>, IGenericRepository<DeviceSession>, IDeviceSessionRepository
    {
        public DeviceSessionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(DeviceSession model)
        {
            await _context.DevicesSessions.AddAsync(model);
        }

        public async Task DeleteAsync(DeviceSession model)
        {
            _context.DevicesSessions.Remove(model);
            await _context.SaveChangesAsync();
        }

        public Task<DeviceSession> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<DeviceSession> FindByIdAsync(int id)
        {
            return await _context.DevicesSessions.FindAsync(id);
        }

        public async Task<DeviceSession> GetDeviceSessionByUserIdAsync(int userId)
        {
            return await _context.DevicesSessions.FirstOrDefaultAsync(i => i.UserId == userId);
        }

        public void Update(DeviceSession model)
        {
            _context.DevicesSessions.Update(model);
        }
    }
}
