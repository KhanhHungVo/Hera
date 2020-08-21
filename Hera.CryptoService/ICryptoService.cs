using Hera.CryptoService.Models.CoinMarketCap;
using Hera.CryptoService.Models.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.CryptoService
{
    public interface ICryptoService
    {
        Task<Response<List<CryptocurrencyWithLatestCode>>> makeAPICall();
        Task<List<CoinBasicInfo>> GetListCoinBasicInfo(int number,string sortOrder="");
    }
}
