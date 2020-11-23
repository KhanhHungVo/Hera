using Hera.Common.Data;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public class OrderEntity : EntityBaseTypeId<int>
    {
        public DateTime OrderDate { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Volume { get; set; }
        public string Reason { get; set; }
        public double InvesmentValue { get; set; }
        public double Cost { get; set; }
        public double CurrentPrice { get; set; }
        public double MarketValue { get; set; }
        public double GainLossPercentage { get; set; }
        public double GainLoss { get; set; }
        public Boolean Status { get; set; }

    }

    public class OrderEntityBuilder : HeraBaseCustomModelBinder<OrderEntity, int>, IHeraCustomModelBinder
    {
        public override void Build(ModelBuilder binder)
        {
            base.BuildBaseProperties(binder);

            binder.Entity<OrderEntity>().ToTable("Orders")
                    .HasKey(u => u.Id);

            binder.Entity<OrderEntity>()
                  .Property(t => t.Symbol)
                  .HasColumnType("VARCHAR(100)")
                  .IsRequired();
            binder.Entity<OrderEntity>()
                  .Property(t => t.Name)
                  .HasColumnType("VARCHAR(100)")
                  .IsRequired();
            binder.Entity<OrderEntity>()
                  .Property(t => t.OrderDate)
                  .HasColumnType("DATE")
                  .IsRequired();
            binder.Entity<OrderEntity>()
                  .Property(t => t.Volume)
                  .HasColumnType("NUMERIC(12,2)")
                  .IsRequired();
        }
    }
}
