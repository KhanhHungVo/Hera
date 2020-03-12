using Hera.Common.Data;
using Hera.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Infrastructure
{
    public interface IUserRepository : IRepositoryBaseTypeId<UserEntity, string>
    {
    }
}
