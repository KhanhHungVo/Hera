using Hera.Common.Data;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public class CoinBasicInfoEntity : EntityBaseTypeId<int>
    {
        public long? MarketCapRanking { get; set; }
        public String Name { get; set; }
        public String Symbol { get; set; }
        public double? CurrentPrice { get; set; }
        public double? PercentChange7D { get; set; }
        public long? MarketCap { get; set; }
        public double? PercentChange24H { get; set; }
        public long? MaxSupply { get; set; }
        public long? CirculatingSupply { get; set; }
        public long? TotalSupply { get; set; }
        public long? Volume24h { get; set; }
    }
    public class CoinBasicInfoEntityBuilder : HeraBaseCustomModelBinder<CoinBasicInfoEntity, int>, IHeraCustomModelBinder
    {
        public override void Build(ModelBuilder binder)
        {
            base.BuildBaseProperties(binder);

            
        }
    }
}
