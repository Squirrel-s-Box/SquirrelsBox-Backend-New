using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Communication
{
    public class PermissionResponse : BaseResponse<Permission>
    {
        public PermissionResponse(string message) : base(message)
        {
        }

        public PermissionResponse(Permission resource) : base(resource)
        {
        }
    }
}
