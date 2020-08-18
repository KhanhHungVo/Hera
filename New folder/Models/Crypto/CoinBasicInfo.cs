using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.CryptoService.Models.Crypto
{
    public class CoinBasicInfo
    {
        public String Name { get; set; }
        public String Symbol { get; set; }
        public double? CurrentPrice { get; set; }
    }
}
