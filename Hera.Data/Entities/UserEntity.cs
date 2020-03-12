using Hera.Common.Data;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hera.Data.Entities
{
    public partial class UserEntity : EntityBaseTypeId<string>
    {
        public UserEntity()
        {
        }

        public float Band { get; set; }

        public short Age { get; set; }

        public DateTime DOB { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string HashedPassword { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class UserEntityBuilder : IHeraCustomModelBinder
    {
        public void Build(ModelBuilder binder)
        {
            binder.Entity<UserEntity>().ToTable("Users")
                    .HasKey(u => u.Id);
        }
    }

    public partial class UserEntity : EntityBaseTypeId<string>
    {
        public void GenerateGuid()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
