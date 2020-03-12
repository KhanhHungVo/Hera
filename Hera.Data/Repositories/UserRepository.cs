using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Data.Repositories
{
    public class UserRepository : RepositoryBaseTypeId<UserEntity, string>, IUserRepository
    {
        public UserRepository(HeraDbContext context) : base(context)
        {

        }
    }
}
