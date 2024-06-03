using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Communication
{
    public class BoxResponse : BaseResponse<Box>
    {
        public BoxResponse(string message) : base(message)
        {
        }

        public BoxResponse(Box resource) : base(resource)
        {
        }
    }
}
