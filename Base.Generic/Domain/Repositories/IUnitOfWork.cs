using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Generic.Domain.Repositories
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        Task CompleteAsync();
    }
}
