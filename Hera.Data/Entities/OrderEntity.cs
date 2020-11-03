using Hera.Common.Data;
using Hera.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public class OrderEntity : EntityBaseTypeId<int>
    {
        DateTime OrderDate { get; set; }
        string Name { get; set; }
        string Symbol { get; set; }
        int Volume { get; set; }
        string Reason { get; set; }
        double Invesment { get; set; }
        double Cost { get; set; }
        double CurrentPrice { get; set; }
        double MarketPrice { get; set; }
        double GainLossPercentage { get; set; }
        double GainLoss { get; set; }
        
    }
}
