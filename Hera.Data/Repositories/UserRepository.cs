using Hera.Data.Entities;
using Hera.Data.Infrastructure;

namespace Hera.Data.Repositories
{
    public class UserRepository : RepositoryBaseTypeId<UserEntity, string>, IUserRepository
    {
        public UserRepository(HeraDbContext context) : base(context)
        {

        }
    }
}
