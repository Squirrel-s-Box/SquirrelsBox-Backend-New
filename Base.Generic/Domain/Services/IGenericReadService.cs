using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Generic.Domain.Services
{
    public interface IGenericReadService<T, R>
    {
        Task<IEnumerable<R>> ListAllByUserCodeAsync(string userCode);
        Task<IEnumerable<R>> ListAllByIdCodeAsync(int id);
    }
    public interface IGenericSearchService
    {
        Task<object> ListFinderAsync(string text, int type);
        Task<object> CounterByUserCodeAsync(string userCode);
    }
}
