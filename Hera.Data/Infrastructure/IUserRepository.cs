using Hera.Common.Data;
using Hera.Data.Entities;

namespace Hera.Data.Infrastructure
{
    public interface IUserRepository : IRepositoryBaseTypeId<UserEntity, string>
    {
    }
}
