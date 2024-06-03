using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Authentication.Domain.Interfaces;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;

namespace SquirrelsBox.Authentication.Persistence.Repositories
{
    public class UserSessionRespository : BaseRepository<AppDbContext>, IGenericRepository<UserSession>, IUserSessionRepository
    {
        public UserSessionRespository(AppDbContext context) : base(context)
        {
        }
        public async Task AddAsync(UserSession model)
        {
            await _context.UsersSessions.AddAsync(model);
        }

        public void Delete(UserSession model)
        {
            _context.UsersSessions.Remove(model);
        }

        public async Task<UserSession> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<UserSession> FindByIdAsync(int id)
        {
            return await _context.UsersSessions.FindAsync(id);
        }

        public async Task<UserSession> GetUserSessionByUserIdAsync(int UserId)
        {
            return await _context.UsersSessions.FirstOrDefaultAsync(i => i.UserId == UserId);
        }

        public async Task<UserSession> GetUserSessionByUserIdAndOldTokenAsync(int UserId, string OldToken)
        {
            return await _context.UsersSessions.FirstOrDefaultAsync(i => i.UserId == UserId && i.OldToken == OldToken);
        }

        public void Update(UserSession model)
        {
            _context.UsersSessions.Update(model);
        }
    }
}
