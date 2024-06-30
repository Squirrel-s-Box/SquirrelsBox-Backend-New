using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Generic.Domain.Repositories
{
    public interface IGenericReadRepository<T>
    {
        Task<IEnumerable<T>> ListAllByUserCodeAsync(string userCode);
        Task<IEnumerable<T>> ListAllByIdAsync(int id);
    }

    public interface IGenericSearchRepository
    {
        Task<object> ListFinderAsync(string text, int type);
        Task<object> CounterByUserCodeAsync(string userCode);
    }
}
