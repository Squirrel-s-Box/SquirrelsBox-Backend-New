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
        Task DeleteAsync(T model);
    }

    public interface IGenericRepositoryWithCascade<T> : IGenericRepository<T>
    {
        Task DeleteCascadeAsync(T model, bool cascade = false);
    }

    public interface IGenericRepositoryWithMassive<T> : IGenericRepository<T>
    {
        Task AddMassiveAsync(ICollection<T> modelList);
    }
}
