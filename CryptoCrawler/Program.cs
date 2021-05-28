using Hera.CryptoService;
using Hera.CryptoService.Models.Crypto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
            
        }
        static async Task MainAsync()
        {
            var cmcService = new CoinMarketCapService();
            var res = await cmcService.GetListCoinBasicInfo(1, 100, "PercentChange7D");
            string resJsonString = JsonConvert.SerializeObject(res, Formatting.Indented);
            Console.WriteLine(resJsonString);
        }
    }
}
