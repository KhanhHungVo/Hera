using Hera.CryptoService.Models.CoinMarketCap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.CryptoService
{
    public interface ICryptoService
    {
        Task<Response<List<CryptocurrencyWithLatestCode>>> makeAPICall();
    }
}
