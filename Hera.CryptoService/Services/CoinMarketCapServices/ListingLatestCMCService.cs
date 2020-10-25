using Hera.Common.Helper;
using Hera.Common.Utils;
using Hera.CryptoService.Models.CoinMarketCap;
using Hera.CryptoService.Models.CoinMarketCapModels;
using Hera.CryptoService.Models.Crypto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hera.CryptoService.Services.CoinMarketCapServices
{
    public class ListingLatestCMCService : CoinMarketCapService, IListingLatestCMCService
    {
        public ListingLatestCMCService() : base()
        {
        }
        /// <summary>
        /// get top n coins from coinmarketcap
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public override async Task<List<CoinBasicInfo>> GetListCoinBasicInfo(int start, int limit, string sortColumn, string sortOrder = "")
        {
            var rs = new List<CoinBasicInfo>();
            var rqParams = new ListingLatestRq()
            {
                Start = start,
                Limit = limit,
                Convert = "USD"
            };
            var listCoins = await SendApiRequest<List<ListingLatestDataRs>>(rqParams, "cryptocurrency/listings/latest");
            if (listCoins != null && !listCoins.Data.IsNullOrEmpty())
            {
                foreach (var item in listCoins.Data)
                {
                    rs.Add(MapToCoinBasicInfo(item));
                }
            }
            rs = SortHelper<CoinBasicInfo>.SortBy(rs, sortColumn, sortOrder).ToList();
            return rs;
        }
    }
}
