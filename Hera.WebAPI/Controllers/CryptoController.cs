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
    public class CryptoController : HeraBaseController
    {
        private readonly ICryptoService _cryptoService;
        //public CryptoController(ICryptoService cryptoService)
        //{
        //    _cryptoService = cryptoService;
        //}

        public CryptoController(IHttpContextAccessor httpContextAccessor, ICryptoService cryptoService
            ) : base(httpContextAccessor)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _cryptoService.makeAPICall();
            return HeraOk(result);
        }
    }
}