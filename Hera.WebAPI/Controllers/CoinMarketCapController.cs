using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hera.Common.WebAPI;
using Hera.CryptoService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hera.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CoinMarketCapController : HeraBaseController
    {
        private readonly ICoinMarketCapService _coinMarketCapService;
        //public CryptoController(ICryptoService cryptoService)
        //{
        //    _cryptoService = cryptoService;
        //}

        public CoinMarketCapController(IHttpContextAccessor httpContextAccessor, ICoinMarketCapService cryptoService
            ) : base(httpContextAccessor)
        {
            _coinMarketCapService = cryptoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _coinMarketCapService.makeAPICall();
            return HeraOk(result);
        }

        [Route("list-coins")]
        public async Task<IActionResult> GetListCoins(int start = 1, int limit = 50, string sortColumn= "MarketCap", string sortOrder="")
        {
            var result = await _coinMarketCapService.GetListCoinBasicInfo(start, limit, sortColumn, sortOrder);
            return HeraOk(result);
        }

        [Route("coin")]
        public async Task<IActionResult> GetCoinBasicInfo(string symbol)
        {
            var result = await _coinMarketCapService.GetCoinBasicInfo(symbol);
            return HeraOk(result);
        }

    }
}