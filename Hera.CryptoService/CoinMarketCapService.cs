using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Hera.CryptoService
{
    public class CoinMarketCapService : ICryptoService
    {
        public const string API_KEY = "b447a55c-e07c-4926-92e7-80ecc22aa461";

        public async Task<string> makeAPICall()
        {
            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = "5000";
            queryString["convert"] = "USD";

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.Headers.Add("Accepts", "application/json");
            var result = await client.DownloadStringTaskAsync(URL.ToString());
            return result;
        }
    }
}
