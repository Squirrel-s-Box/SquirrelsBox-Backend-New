using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Domain.Interfaces
{
    public interface IActionLogRepository
    {
        Task<IEnumerable<ActionLog>> ReadActionLog(string userCode);
    }
}
