using Hera.Common.Core;
using Hera.Common.Data;

namespace Hera.Services
{
    public interface IServiceBase<T> : IServiceBaseTypeId<T, long> where T : class, IEntityTypeId<long>
    {
    }
}
