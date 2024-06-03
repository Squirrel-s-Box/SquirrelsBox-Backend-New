using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Communication
{
    public class SpecResponse : BaseResponse<Spec>
    {
        public SpecResponse(string message) : base(message)
        {
        }

        public SpecResponse(Spec resource) : base(resource)
        {
        }
    }
}
