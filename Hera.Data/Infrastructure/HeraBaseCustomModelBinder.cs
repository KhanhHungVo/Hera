using Hera.Common.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Infrastructure
{
    public abstract class HeraBaseCustomModelBinder<T, TId> : IHeraCustomModelBinder where T : EntityBaseTypeId<TId>
    {
        public abstract void Build(ModelBuilder binder);

        public virtual void BuildBaseProperties(ModelBuilder binder)
        {
            binder.Entity<T>()
                  .Property(e => e.CreatedDate)
                  .HasColumnType("TIMESTAMP WITH TIME ZONE")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            binder.Entity<T>()
                  .Property(e => e.IsActive)
                  .HasColumnType("BOOLEAN")
                  .HasDefaultValueSql("TRUE");

            binder.Entity<T>()
                  .Property(e => e.IsDeleted)
                  .HasColumnType("BOOLEAN")
                  .HasDefaultValueSql("FALSE");

            binder.Entity<T>()
                  .Property(e => e.UpdatedDate)
                  .HasColumnType("TIMESTAMP WITH TIME ZONE")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .IsRequired(false);
        }
    }
}
