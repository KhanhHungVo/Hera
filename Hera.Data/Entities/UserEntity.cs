using Hera.Common.Data;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Hera.Data.Entities
{
    public partial class UserEntity : EntityBaseTypeId<string>
    {
        public UserEntity()
        {
        }

        public UserEntity(string userId)
        {
            Id = userId;
        }

        public float Band { get; set; }

        public short Age { get; set; }

        public DateTime? DOB { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string HashedPassword { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool Onboarding { get; set; }

        public ICollection<UserTokenEntity> UserTokens { get; set; }

        public ICollection<TopicsUserInterestEntity> TopicsUserInterest { get; set; }
    }

    public class UserEntityBuilder : HeraBaseCustomModelBinder<UserEntity, string>, IHeraCustomModelBinder
    {
        public override void Build(ModelBuilder binder)
        {
            base.BuildBaseProperties(binder);

            binder.Entity<UserEntity>().ToTable("Users")
                    .HasKey(u => u.Id);

            binder.Entity<UserEntity>()
                  .Property(u => u.Id)
                  .HasDefaultValueSql("uuid_generate_v4()");

            binder.Entity<UserEntity>()
                  .Property(u => u.Band)
                  .HasColumnType("NUMERIC(3,1)")
                  .HasDefaultValue(0.0f);

            binder.Entity<UserEntity>()
                  .Property(u => u.Age)
                  .HasColumnType("NUMERIC(3)")
                  .HasDefaultValue(0);

            binder.Entity<UserEntity>()
                 .Property(u => u.DOB)
                 .HasColumnType("DATE")
                 .IsRequired(false);

            binder.Entity<UserEntity>()
                  .Property(u => u.Username)
                  .HasColumnType("VARCHAR(255)")
                  .IsRequired();

            binder.Entity<UserEntity>()
                  .Property(u => u.FirstName)
                  .HasColumnType("VARCHAR(100)")
                  .IsRequired();

            binder.Entity<UserEntity>()
                  .Property(u => u.LastName)
                  .HasColumnType("VARCHAR(100)")
                  .HasDefaultValue("");

            binder.Entity<UserEntity>()
                  .Property(u => u.HashedPassword)
                  .HasColumnType("VARCHAR");

            binder.Entity<UserEntity>()
                  .Property(u => u.Email)
                  .HasColumnType("VARCHAR(255)")
                  .IsRequired();

            binder.Entity<UserEntity>()
                  .Property(u => u.PhoneNumber)
                  .HasColumnType("VARCHAR(50)")
                  .IsRequired(false);

            binder.Entity<UserEntity>()
                  .Property(u => u.Onboarding)
                  .HasColumnType("BOOLEAN")
                  .HasDefaultValueSql("TRUE");
        }
    }
}
