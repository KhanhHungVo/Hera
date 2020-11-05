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

        void Add<TEntity, TTypeId>(TEntity entity) where TEntity : class, IEntityTypeId<TTypeId>;

        void AddRange(IEnumerable<T> entities);

        IDbContextTransaction BeginTransaction();

        void SetEntityState<TEntity, TTypeId>(TEntity entity, EntityState state) where TEntity : class, IEntityTypeId<TTypeId>;

        void SetEntityPropertiesHasModified<TEntity, TTypeId>(TEntity entity, IEnumerable<string> modifiedPropertiesName) where TEntity : class, IEntityTypeId<TTypeId>;

        void SaveChanges();

        Task SaveChangesAsync();

        void Delete(T entity);

        public Task<T> DeleteAsync(TId id);
        public Task<T> CreateAsync(T entity);

        public Task<T> GetAsync(TId id);

        public Task<List<T>> GetAsync();

        public Task<T> UpdateAsync(T entity);
    }
}
