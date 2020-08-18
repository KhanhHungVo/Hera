using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.CryptoService.Models.CoinMarketCap
{
    public class Response<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}
