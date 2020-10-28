using Hera.Common.Data;

namespace Hera.Data.Infrastructure
{
    public interface IRepositoryBase<T> : IRepositoryBaseTypeId<T, int> where T : IEntityTypeId<int>
    {
    }
}
