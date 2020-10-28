using Hera.Common.Data;

namespace Hera.Services
{
    public class ServiceBase<T> : ServiceBaseTypeId<T, int>, IServiceBase<T> where T : class, IEntityTypeId<int>
    {
        public ServiceBase(IRepositoryBaseTypeId<T, int> repository) : base(repository)
        {
        }
    }
}
