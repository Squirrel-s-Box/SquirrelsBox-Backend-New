using Base.Generic.Domain.Services.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Communication
{
    public class ActionLogResponse : BaseResponse<ActionLog>
    {
        public ActionLogResponse(string message) : base(message)
        {
        }

        public ActionLogResponse(ActionLog resource) : base(resource)
        {
        }
    }
}
