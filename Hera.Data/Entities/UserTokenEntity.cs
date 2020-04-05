using Hera.Common.Data;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public class UserTokenEntity : EntityBaseTypeId<string>
    {
        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiredTime { get; set; }

        public DateTime AccessTokenIssuedTime { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiredTime { get; set; }

        public DateTime RefreshTokenIssuedTime { get; set; }

        public string UserId { get; set; }

        public UserEntity User { get; set; }
    }

    public class UserTokenEntityBuilder : HeraBaseCustomModelBinder<UserTokenEntity, string>, IHeraCustomModelBinder
    {
        public override void Build(ModelBuilder binder)
        {
            base.BuildBaseProperties(binder);

            binder.Entity<UserTokenEntity>().ToTable("UserToken")
                    .HasKey(u => u.Id);

            binder.Entity<UserTokenEntity>()
                  .Property(u => u.Id)
                  .HasDefaultValueSql("uuid_generate_v4()");

            binder.Entity<UserTokenEntity>()
                  .Property(ut => ut.AccessToken)
                  .IsRequired();

            binder.Entity<UserTokenEntity>()
                  .Property(ut => ut.RefreshToken)
                  .IsRequired();

            binder.Entity<UserTokenEntity>()
                  .HasOne(ut => ut.User)
                  .WithMany(u => u.UserTokens)
                  .HasForeignKey(ut => ut.UserId)
                  .IsRequired();
        }
    }
}
