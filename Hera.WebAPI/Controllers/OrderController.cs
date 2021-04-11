using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Common.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hera.WebAPI.Controllers
{
    [Route("api/orders")]
    //[Authorize]
    public class OrderController : HeraBaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IHttpContextAccessor httpContextAccessor, IOrderService orderService) : base(httpContextAccessor)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderViewModel model)
        {
            return HeraOk(await _orderService.CreateAsync(model));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderViewModel model)
        {
            return HeraOk(await _orderService.UpdateAsync(model));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return HeraOk(await _orderService.GetAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HeraOk(await _orderService.GetAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return HeraOk(await _orderService.DeleteAsync(id));
        }


    }
}
