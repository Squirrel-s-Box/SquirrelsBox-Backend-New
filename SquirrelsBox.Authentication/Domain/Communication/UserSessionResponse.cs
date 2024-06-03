using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Communication
{
    public class UserSessionResponse : BaseResponse<UserSession>
    {
        public UserSessionResponse(string message) : base(message)
        {
        }

        public UserSessionResponse(UserSession resource) : base(resource)
        {
        }
    }
}
