using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.ViewModels.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hera.WebAPI.Controllers
{
    public class OrderController : HeraBaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IHttpContextAccessor httpContextAccessor, IOrderService orderService) : base(httpContextAccessor)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> Create([FromBody]OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }
            return HeraOk(await _orderService.CreateAsync(model));
        }


      
    }
}
