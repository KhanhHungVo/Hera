using Hera.Common.Data;

namespace Hera.Data.Infrastructure
{
    public interface IRepositoryBase<T> : IRepositoryBaseTypeId<T, long> where T : IEntityTypeId<long>
    {
    }
}
