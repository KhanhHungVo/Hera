using Hera.CryptoService.Models.CoinMarketCap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Hera.CryptoService
{
    public class CoinMarketCapService : ICryptoService
    {
        public const string API_KEY = "b447a55c-e07c-4926-92e7-80ecc22aa461";
        public const string BASE_URL = "https://pro-api.coinmarketcap.com";

        public async Task<Response<List<CryptocurrencyWithLatestCode>>> makeAPICall()
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
            var result = JsonConvert.DeserializeObject<Response<List<CryptocurrencyWithLatestCode>>>(content);
            
            
            return result; 
        }
    }
}
