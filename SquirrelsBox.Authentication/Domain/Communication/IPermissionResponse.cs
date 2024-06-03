using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Communication
{
    public interface IPermissionResponse
    {
        bool Success { get; }
        Permission PermissionResource { get; }
    }
}
