using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Communication
{
    public class SectionItemRelationshipResponse : BaseResponse<SectionItemRelationship>
    {
        public SectionItemRelationshipResponse(string message) : base(message)
        {
        }

        public SectionItemRelationshipResponse(SectionItemRelationship resource) : base(resource)
        {
        }
    }
}
