using Hera.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hera.WebAPI.Helper
{
    public static class ExtensionMethods
    {
        public static User WithoutPassword(this User user)
        {
            user.Password = null;
            return user;
        }
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

    }
}
