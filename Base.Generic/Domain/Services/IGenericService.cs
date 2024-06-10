using Base.Generic.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Generic.Domain.Services
{
    public interface IGenericService<T, R>
    {
        Task<R> SaveAsync(T model);
        Task<R> FindByIdAsync(int id);
        Task<R> FindByCodeAsync(string value);
        Task<R> UpdateAsync(int id, T model);
        Task<R> DeleteAsync(int id);
    }

    public interface IGenericServiceWithCascade<T, R> : IGenericService<T, R>
    {
        Task<R> DeleteCascadeAsync(int id, bool cascade = false);
    }

    public interface IGenericServiceWithMassive<T, R>
    {
        Task<R> SaveMassiveAsync(ICollection<T> modelList);
        Task<R> UpdateMassiveAsync(ICollection<T> modelList);
        Task<R> DeleteteMassiveAsync(ICollection<int> ids);
    }
}
