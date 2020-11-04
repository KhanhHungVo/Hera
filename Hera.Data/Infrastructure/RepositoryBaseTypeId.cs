using Hera.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Data.Infrastructure
{
    public class RepositoryBaseTypeId<T, TId> : IRepositoryBaseTypeId<T, TId> where T : class, IEntityTypeId<TId>
    {
        protected DbContext Context { get; }

        protected DbSet<T> DbSet { get; }

        public RepositoryBaseTypeId(HeraDbContext dbContext)
        {
            Context = dbContext;
            DbSet = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Add<TEntity, TTypeId>(TEntity entity) where TEntity : class, IEntityTypeId<TTypeId>
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
        }

        public async Task<T> CreateAsync(T entity)
        {
            DbSet.Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetById(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T> DeleteAsync(TId id)
        {
            var entity = await DbSet.FindAsync(id);
            if(entity == null)
            {
                return entity;
            }
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T> Query()
        {
            return DbSet;
        }

        public IQueryable<TEntity> Query<TEntity, TTypeId>() where TEntity : class, IEntityTypeId<TTypeId>
        {
            return Context.Set<TEntity>();
        }

        public IQueryable<T> QueryAsNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        public IQueryable<TEntity> QueryAsNoTracking<TEntity, TTypeId>() where TEntity : class, IEntityTypeId<TTypeId>
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public void SetEntityState<TEntity, TTypeId>(TEntity entity, EntityState state) where TEntity : class, IEntityTypeId<TTypeId>
        {            
            Context.Entry(entity).State = state;
        }

        public void SetEntityPropertiesHasModified<TEntity, TTypeId>(TEntity entity, IEnumerable<string> modifiedPropertiesName) where TEntity : class, IEntityTypeId<TTypeId>
        {
            var entryEntity = Context.Entry(entity);

            foreach (var propertyName in modifiedPropertiesName)
            {
                entryEntity.Property(propertyName).IsModified = true;
            }
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
