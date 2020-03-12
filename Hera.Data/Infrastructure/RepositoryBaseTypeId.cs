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

        public void AddRange(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
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
