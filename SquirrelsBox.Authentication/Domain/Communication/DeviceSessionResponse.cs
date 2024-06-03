using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Communication
{
    public class DeviceSessionResponse : BaseResponse<DeviceSession>
    {
        public DeviceSessionResponse(string message) : base(message)
        {
        }

        public DeviceSessionResponse(DeviceSession resource) : base(resource)
        {
        }
    }
}
