using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Interfaces
{
    public interface IDeviceSessionRepository
    {
        Task<DeviceSession> GetDeviceSessionByUserIdAsync(int UserId);
    }
}
