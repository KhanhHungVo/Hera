using Hera.Common.Data;
using Hera.Data.Entities;
using System.Threading.Tasks;

namespace Hera.Data.Infrastructure
{
    public interface IUserRepository : IRepositoryBaseTypeId<UserEntity, string>
    {
        //Task<UserEntity> UpdateAsync(UserEntity entity);
    }
}
