using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.CryptoService.Models.CoinMarketCapModels
{
    public class ListingLatestRq
    {
        [JsonProperty("start")]
        public int? Start { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("convert")]
        public string Convert { get; set; }

        [JsonProperty("sort")]
        public int? Sort { get; set; }

        [JsonProperty("sort_dir")]
        public int? SortDir { get; set; }

        [JsonProperty("cryptocurrency_type")]
        public int? CryptocurrencyType { get; set; }
    }
}
