using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hera.Common.Data
{
    public interface IRepositoryBaseTypeId<T, TId> where T : IEntityTypeId<TId>
    {
        IQueryable<T> Query();

        IQueryable<TEntity> Query<TEntity, TTypeId>() where TEntity: class, IEntityTypeId<TTypeId>;

        IQueryable<T> QueryAsNoTracking();

        IQueryable<TEntity> QueryAsNoTracking<TEntity, TTypeId>() where TEntity : class, IEntityTypeId<TTypeId>;

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        IDbContextTransaction BeginTransaction();

        void SetEntityState<TEntity, TTypeId>(TEntity entity, EntityState state) where TEntity : class, IEntityTypeId<TTypeId>;

        void SetEntityPropertiesHasModified<TEntity, TTypeId>(TEntity entity, IEnumerable<string> modifiedPropertiesName) where TEntity : class, IEntityTypeId<TTypeId>;

        void SaveChanges();

        Task SaveChangesAsync();

        void Delete(T entity);
    }
}
