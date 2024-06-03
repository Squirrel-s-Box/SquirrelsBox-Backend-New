using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Interfaces
{
    public interface IUserSessionRepository
    {
        Task<UserSession> GetUserSessionByUserIdAndOldTokenAsync(int UserId, string OldToken);
        Task<UserSession> GetUserSessionByUserIdAsync(int UserId);
    }
}
