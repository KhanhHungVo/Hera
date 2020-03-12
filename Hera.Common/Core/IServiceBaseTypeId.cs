using Hera.Common.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Common.Core
{
    public interface IServiceBaseTypeId<T, TId> where T : class, IEntityTypeId<TId>
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        IDbContextTransaction BeginTransaction();

        void SaveChanges();

        Task SaveChangesAsync();

        void Delete(T entity);
    }
}
