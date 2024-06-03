using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Communication
{
    public class UserDataResponse : BaseResponse<UserData>
    {
        public UserDataResponse(string message) : base(message)
        {
        }

        public UserDataResponse(UserData resource) : base(resource)
        {
        }
    }
}
