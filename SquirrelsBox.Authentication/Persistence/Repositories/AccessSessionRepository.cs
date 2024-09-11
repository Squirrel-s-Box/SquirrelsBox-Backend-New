using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Authentication.Domain.Interfaces;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;

namespace SquirrelsBox.Authentication.Persistence.Repositories
{
    public class AccessSessionRepository : BaseRepository<AppDbContext>, IGenericRepository<AccessSession>, IAccessSessionRepository
    {
        public AccessSessionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(AccessSession model)
        {
            await _context.AccessSessions.AddAsync(model);
        }

        public async Task DeleteAsync(AccessSession model)
        {
            _context.AccessSessions.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> FindByEmail(string email)
        {
            var user = await _context.UsersData.FirstOrDefaultAsync(i => i.Email == email);
            return user != null;
        }

        public async Task<AccessSession> FindByCodeAsync(string value)
        {
            return await _context.AccessSessions.FirstOrDefaultAsync(i => i.Code == value);
        }

        public async Task<AccessSession> FindByIdAsync(int id)
        {
            return await _context.AccessSessions.FindAsync(id);
        }

        public void Update(AccessSession model)
        {
            _context.AccessSessions.Update(model);
        }

        public async Task<bool> VerifyAndReplaceRefreshTokenAsync(string refreshToken, string newRefreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(newRefreshToken))
            {
                throw new ArgumentException("Refresh tokens cannot be null or whitespace.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                //try
                //{
                var session = await _context.AccessSessions
                                            .FirstOrDefaultAsync(i => i.RefreshToken == refreshToken);

                if (session == null)
                {
                    return false;
                }

                // Replace the old refresh token with the new one
                session.RefreshToken = newRefreshToken;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
                //}
                //catch (DbUpdateConcurrencyException ex)
                //{
                //    // Handle concurrency issues
                //    _logger.LogError(ex, "Concurrency issue occurred while replacing the refresh token.");
                //    await transaction.RollbackAsync();
                //    return false;
                //}
                //catch (Exception ex)
                //{
                //    // Log the exception
                //    _logger.LogError(ex, "An error occurred while replacing the refresh token.");
                //    await transaction.RollbackAsync();
                //    return false;
                //}
            }
        }

        public async Task<AccessSession> LogIn(AccessSession model)
        {
            var user = await _context.AccessSessions.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
        }
    }
}
