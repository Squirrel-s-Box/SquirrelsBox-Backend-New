using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Communication
{
    public class SharedBoxResponse : BaseResponse<SharedBox>
    {
        public SharedBoxResponse(string message) : base(message)
        {
        }

        public SharedBoxResponse(SharedBox resource) : base(resource)
        {
        }
    }
}
