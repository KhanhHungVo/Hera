using Hera.Common.Data;
using Hera.Data.Infrastructure;
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
        public double Invesment { get; set; }
        public double Cost { get; set; }
        public double CurrentPrice { get; set; }
        public double MarketPrice { get; set; }
        public double GainLossPercentage { get; set; }
        public double GainLoss { get; set; }
        
    }
}
