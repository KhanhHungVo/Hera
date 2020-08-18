using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.CryptoService.Models.Crypto
{
    public class CoinBasicInfo
    {
        String Name { get; set; }
        String Symbol { get; set; }
        decimal CurrentPrice { get; set; }
    }
}
