using Hera.Common.Data;

namespace Hera.Data.Infrastructure
{
    public interface IRepositoryBaseTypeId<T> : IRepositoryBaseTypeId<T, long> where T : IEntityTypeId<long>
    {
    }
}
