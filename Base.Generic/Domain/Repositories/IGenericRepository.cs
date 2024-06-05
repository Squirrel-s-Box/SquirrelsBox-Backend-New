using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Generic.Domain.Repositories
{
    public interface IGenericRepository<T>
    {
        Task AddAsync(T model);
        Task<T> FindByIdAsync(int id);
        Task<T> FindByCodeAsync(string value);
        void Update(T model);
        void Delete(T model);
    }

    public interface IGenericRepositoryWithCascade<T> : IGenericRepository<T>
    {
        void DeleteCascade(T model, bool cascade = false);
    }
}
