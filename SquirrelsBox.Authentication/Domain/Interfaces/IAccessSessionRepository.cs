using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Interfaces
{
    public interface IAccessSessionRepository
    {
        Task<bool> VerifyAndReplaceRefreshTokenAsync(string actualRefreshToken, string newRefreshToken);
    }
}
