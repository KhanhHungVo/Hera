using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.CryptoService.Models.Crypto
{
    public class CoinBasicInfo
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
}
