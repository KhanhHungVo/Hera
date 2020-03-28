using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Common.Data
{
    public interface IRepositoryBaseTypeId<T, TId> where T : IEntityTypeId<TId>
    {
        IQueryable<T> Query();

        IQueryable<T> QueryAsNoTracking();

        IQueryable<TEntity> QueryAsNoTracking<TEntity, TTypeId>() where TEntity : class, IEntityTypeId<TTypeId>;

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        IDbContextTransaction BeginTransaction();

        void SaveChanges();

        Task SaveChangesAsync();

        void Delete(T entity);
    }
}
