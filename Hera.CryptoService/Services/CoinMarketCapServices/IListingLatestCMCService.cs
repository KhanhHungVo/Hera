using Hera.CryptoService.Models.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.CryptoService.Services.CoinMarketCapServices
{
    interface IListingLatestCMCService
    {
        public Task<List<CoinBasicInfo>> GetListCoinBasicInfo(int start, int limit, string sortColumn, string sortOrder = "");
    }
}
