using Hera.Common.Data;

namespace Hera.Services
{
    public class ServiceBase<T> : ServiceBaseTypeId<T, long>, IServiceBase<T> where T : class, IEntityTypeId<long>
    {
        public ServiceBase(IRepositoryBaseTypeId<T, long> repository) : base(repository)
        {
        }
    }
}
