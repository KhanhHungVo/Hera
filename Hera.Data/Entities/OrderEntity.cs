﻿using Hera.Common.Data;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public class OrderEntity : EntityBaseTypeId<int>
    {
        //public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Volume { get; set; }
        public string Reason { get; set; }
        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public double InvestmentValue { get; set; }

        /// <summary>
        /// Giá mua vào/ giá vốn trên 1 đơn vị
        /// </summary>
        public double OrderPrice { get; set; }

        /// <summary>
        /// Giá hiện tại trên 1 đơn vị
        /// </summary>
        public double CurrentPrice { get; set; }
        /// <summary>
        /// Tổng giá trị hiện tại theo giá thị trường
        /// </summary>
        public double MarketValue { get; set; }
        public double GainLossPercentage { get; set; }
        public double GainLoss { get; set; }
        public Boolean Done { get; set; }
        public int Type { get; set; }

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
