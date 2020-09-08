using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.CryptoService.Models.Crypto
{
    public class CoinBasicInfo
    {
        public int MarketCapRanking { get; set; }
        public String Name { get; set; }
        public String Symbol { get; set; }
        public double? CurrentPrice { get; set; }
        public double? PercentChange7D { get; set; }
        public long? MarketCap { get; set; }
        public double? PercentChange24H { get; set; }
    }
}
