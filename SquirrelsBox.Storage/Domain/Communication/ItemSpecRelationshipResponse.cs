using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Communication
{
    public class ItemSpecRelationshipResponse : BaseResponse<ItemSpecRelationship>
    {
        public ItemSpecRelationshipResponse(string message) : base(message)
        {
        }

        public ItemSpecRelationshipResponse(ItemSpecRelationship resource) : base(resource)
        {
        }
    }
}
