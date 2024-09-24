using SquirrelsBox.Storage.Domain.Communication;

namespace SquirrelsBox.Storage.Domain.Interfaces
{
    public interface IActionLogService
    {
        Task<IEnumerable<ActionLogResponse>> ReadActionLog(string userCode);
    }
}
