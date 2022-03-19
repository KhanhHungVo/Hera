using System.Threading.Tasks;
using Hera.Common.WebAPI;
using Hera.CryptoService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hera.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CoinMarketCapController : HeraBaseController
    {
        private readonly ICoinMarketCapService _coinMarketCapService;
        private readonly ILogger logger;

        public CoinMarketCapController(IHttpContextAccessor httpContextAccessor, ICoinMarketCapService cryptoService, ILogger<CoinMarketCapController> logger
            ) : base(httpContextAccessor)
        {
            _coinMarketCapService = cryptoService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _coinMarketCapService.makeAPICall();
            logger.LogInformation("test");
            return HeraOk(result);
        }

        [Route("list-coins")]
        [HttpGet]
        public async Task<IActionResult> GetListCoins(int start = 1, int limit = 50, string sortColumn= "MarketCap", string sortOrder="")
        {
            var result = await _coinMarketCapService.GetListCoinBasicInfo(start, limit, sortColumn, sortOrder);
            return HeraOk(result);
        }

        [Route("coin")]
        [HttpGet]
        public async Task<IActionResult> GetCoinBasicInfo(string symbol)
        {
            var result = await _coinMarketCapService.GetCoinBasicInfo(symbol);
            return HeraOk(result);
        }
        
    }
}