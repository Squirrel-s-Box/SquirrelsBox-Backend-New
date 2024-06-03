using Base.Generic.Domain.Repositories;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;

namespace SquirrelsBox.Authentication.Persistence.Repositories
{
    public class UserDataRepository : BaseRepository<AppDbContext>, IGenericRepository<UserData>
    {
        public UserDataRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(UserData model)
        {
            await _context.UsersData.AddAsync(model);
        }

        public void Delete(UserData model)
        {
            _context.UsersData.Remove(model);
        }

        public async Task<UserData> FindByCodeAsync(string value)
        {
            return await _context.UsersData.FirstOrDefaultAsync(i => i.UserCode == value);
        }

        public async Task<UserData> FindByIdAsync(int id)
        {
            return await _context.UsersData.FindAsync(id);
        }

        public void Update(UserData model)
        {
            _context.UsersData.Update(model);
        }
    }
}
