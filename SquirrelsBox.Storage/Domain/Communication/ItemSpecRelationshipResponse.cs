using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Communication
{
    public class ItemSpecRelationshipResponse : BaseResponse<Spec>
    {
        public ItemSpecRelationshipResponse(string message) : base(message)
        {
        }

        public ItemSpecRelationshipResponse(Spec resource) : base(resource)
        {
        }
        public ItemSpecRelationshipResponse(ICollection<Spec> resource) : base(resource)
        {
        }
        public ItemSpecRelationshipResponse(ICollection<int> resource) : base(resource)
        {
        }
    }
}
