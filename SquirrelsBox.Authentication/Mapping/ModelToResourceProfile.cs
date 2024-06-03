using AutoMapper;
using Base.Generic.Domain.Services.Communication;
using Base.Generic.Resources;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Resource;

namespace SquirrelsBox.Authentication.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<AssignedPermissionResponse, ListAssignedPermission>();
            CreateMap<UserData, UserDataResource>();

            //Validation Resource
            CreateMap(typeof(BaseResponse<>), typeof(ValidationResource));
        }
    }
}
