using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Communication
{
    public class BoxSectionRelationshipResponse : BaseResponse<BoxSectionRelationship>
    {
        public BoxSectionRelationshipResponse(string message) : base(message)
        {
        }

        public BoxSectionRelationshipResponse(BoxSectionRelationship resource) : base(resource)
        {
        }
    }
}
