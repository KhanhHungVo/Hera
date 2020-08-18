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
    public class CoinMarketCapService : ICryptoService
    {
        public const string API_KEY = "b447a55c-e07c-4926-92e7-80ecc22aa461";
        public const string BASE_URL = "https://pro-api.coinmarketcap.com/v1/";

        public readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri(BASE_URL)
        };

        public CoinMarketCapService()
        {
            HttpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
            HttpClient.DefaultRequestHeaders.Add("Accepts", "application/json");
        }
        public async Task<List<CoinBasicInfo>> GetListCoinBasicInfo(int number)
        {
            //var rqParams = new ListingLatestParameters() { 
            //    Start=1,
            //    Limit=number,
            //    Convert="USD"
            //};
            //var listCoins = await SendApiRequest<List<CryptocurrencyWithLatestCode>>(rqParams, "cryptocurrency/listings/latest");
            //if(listCoins != null && listCoins.Data.IsNullOrEmpty())
            //{

            //}
            return null;
        }

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
        private async Task<Response<T>> SendApiRequest<T>(object requestParams, string endpoint)
        {
            var queryParams = ConvertToQueryParams(requestParams);
            var endpointWithParams = $"{endpoint}{queryParams}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpointWithParams);
            var responseMessage = await HttpClient.SendAsync(requestMessage);
            responseMessage.EnsureSuccessStatusCode();
            var content = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<T>>(content);
        }

        private static string ConvertToQueryParams(object parameters)
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
        public CoinBasicInfo MapToCoinBasicInfo(CryptocurrencyWithLatestCode item)
        {
            var dto = new CoinBasicInfo();
            dto.Name = item.Name;
            dto.Symbol = item.Symbol;
            dto.CurrentPrice = item.Quote["USD"].Price;
            return dto;
        }
    }
}
