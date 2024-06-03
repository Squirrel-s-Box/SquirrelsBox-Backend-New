using Base.Generic.Domain.Services.Communication;

namespace SquirrelsBox.Authentication.Domain.Communication
{
    public class AccessSessionResponse : BaseResponse<Models.AccessSession>
    {
        public string Token { get; private set; }
        public string RefreshToken { get; private set; }
        public AccessSessionResponse(string message) : base(message)
        {
        }

        public AccessSessionResponse(Models.AccessSession resource) : base(resource)
        {
        }

        public AccessSessionResponse(Models.AccessSession resource, string token) : base(resource)
        {
            Token = token;
        }
    }
}
