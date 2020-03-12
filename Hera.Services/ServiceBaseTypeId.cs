using Hera.Common.Core;
using Hera.Common.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hera.Services
{
    public class ServiceBaseTypeId<T, TId> : IServiceBaseTypeId<T, TId> where T : class, IEntityTypeId<TId>
    {
        protected readonly IRepositoryBaseTypeId<T, TId> _repository;

        public ServiceBaseTypeId(IRepositoryBaseTypeId<T, TId> repository)
        {
            _repository = repository;
        }
        public void Add(T entity)
        {
            _repository.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _repository.AddRange(entities);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _repository.BeginTransaction();
        }

        public void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _repository.SaveChangesAsync();
        }
    }
}
