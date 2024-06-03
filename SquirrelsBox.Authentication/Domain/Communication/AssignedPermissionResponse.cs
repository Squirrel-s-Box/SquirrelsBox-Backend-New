using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Communication
{
    public class AssignedPermissionResponse : BaseResponse<AssignedPermission>, IPermissionResponse
    {
        public AssignedPermissionResponse(string message) : base(message)
        {
        }

        public AssignedPermissionResponse(AssignedPermission resource) : base(resource)
        {
        }

        public Permission PermissionResource => Resource?.Permission;
    }
}
