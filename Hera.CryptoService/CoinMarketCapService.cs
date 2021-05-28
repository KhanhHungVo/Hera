using Hera.Common.Helper;
using Hera.Common.Utils;
using Hera.CryptoService.Models.CoinMarketCap;
using Hera.CryptoService.Models.CoinMarketCapModels;
using Hera.CryptoService.Models.Crypto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Hera.CryptoService
{
    public class CoinMarketCapService : ICoinMarketCapService
    {
        public const string API_KEY = "b447a55c-e07c-4926-92e7-80ecc22aa461";
        public const string BASE_URL = "https://pro-api.coinmarketcap.com/v1/";

        public readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri(BASE_URL)
        };

        public CoinMarketCapService():base()
        {
            HttpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
            HttpClient.DefaultRequestHeaders.Add("Accepts", "application/json");
        }
        /// <summary>
        /// get top n coins from coinmarketcap
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public virtual async Task<List<CoinBasicInfo>> GetListCoinBasicInfo(int start, int limit, string sortColumn = "MarketCap", string sortOrder = "")
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
        public async Task<CoinBasicInfo> GetCoinBasicInfo(string symbol)
        {
            var rs = new CoinBasicInfo();
            var rqParams = new ListingLatestDataRs()
            {
                Symbol = symbol
            };
            var coin = await SendApiRequest<Dictionary<string,QuoteLatestDataRs>>(rqParams, "cryptocurrency/quotes/latest");
            rs = MapToCoinBasicInfo(coin.Data);
            return rs;
        }


        public async Task<Response<List<ListingLatestDataRs>>> makeAPICall()
        {
            var URL = new UriBuilder($"{BASE_URL}/v1/cryptocurrency/listings/latest");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = "50";
            queryString["convert"] = "USD";

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.Headers.Add("Accepts", "application/json");
            var content = await client.DownloadStringTaskAsync(URL.ToString());
            var result = JsonConvert.DeserializeObject<Response<List<ListingLatestDataRs>>>(content);
            return result; 
        }
        public async Task<Response<T>> SendApiRequest<T>(object requestParams, string endpoint)
        {
            var queryParams = ConvertToQueryParams(requestParams);
            var endpointWithParams = $"{endpoint}{queryParams}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpointWithParams);
            var responseMessage = await HttpClient.SendAsync(requestMessage);
            responseMessage.EnsureSuccessStatusCode();
            var content = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<T>>(content);
        }

        public static string ConvertToQueryParams(object parameters)
        {
            var properties = parameters.GetType().GetRuntimeProperties();
            var encodedValues = properties
                .Select(x => new
                {
                    Name = x.Name.ToLower(),
                    Value = x.GetValue(parameters)
                })
                .Where(x => x.Value != null)
                .Select(x => $"{x.Name}={System.Net.WebUtility.UrlEncode(x.Value.ToString())}")
                // prepend ? for the first param, & for the rest
                .Select((x, i) => i > 0 ? $"&{x}" : $"?{x}");

            return string.Join(string.Empty, encodedValues);
        }
        public CoinBasicInfo MapToCoinBasicInfo(ListingLatestDataRs item)
        {
            var dto = new CoinBasicInfo();
            dto.MarketCapRanking = item.CmcRank;
            dto.Name = item.Name;
            dto.Symbol = item.Symbol;
            dto.CurrentPrice = item.Quote["USD"].Price;
            dto.MarketCap = item.Quote["USD"].MarketCap;
            dto.PercentChange24H = item.Quote["USD"].PercentChange24H;
            dto.PercentChange7D = item.Quote["USD"].PercentChange7D;
            dto.MaxSupply = item.MaxSupply;
            dto.TotalSupply = item.TotalSupply;
            dto.CirculatingSupply = item.CirculatingSupply;
            dto.Volume24h = item.Quote["USD"]?.Volume24H;
            dto.CirculatingSupply = item.CirculatingSupply;
            return dto;
        }
        public CoinBasicInfo MapToCoinBasicInfo(Dictionary<string,QuoteLatestDataRs> dict)
        {
            var item = dict.First().Value;
            var dto = new CoinBasicInfo();
            dto.MarketCapRanking = item.cmc_rank ;
            dto.Name = item.name;
            dto.Symbol = item.symbol;
            dto.CurrentPrice = item.Quote["USD"].Price;
            dto.MarketCap = item.Quote["USD"].MarketCap;
            dto.PercentChange24H = item.Quote["USD"].PercentChange24H;
            dto.PercentChange7D = item.Quote["USD"].PercentChange7D;
            dto.MaxSupply = item.max_supply;
            dto.TotalSupply = item.total_supply;
            dto.CirculatingSupply = item.circulating_supply;
            dto.Volume24h = item.Quote["USD"]?.Volume24H;
            return dto;
        }

    }
}
