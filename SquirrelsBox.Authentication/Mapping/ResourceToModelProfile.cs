using AutoMapper;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Resource;

namespace SquirrelsBox.Authentication.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveAssignedPermissionResource, AssignedPermission>();


            CreateMap<SaveTokenResource, SaveFoundTokenByUserIdResource>();
            CreateMap<SaveFoundTokenByUserIdResource, DeviceSession>();
            CreateMap<SaveFoundTokenByUserIdResource, UserSession>();
            CreateMap<SaveTokenResource, DeviceSession>();
            CreateMap<UpdateUserSessionResource, UserSession>();
            CreateMap<UpdateUserResource, AccessSession>();

            CreateMap<SaveUserDataResource, UserData>();
        }
    }
}
