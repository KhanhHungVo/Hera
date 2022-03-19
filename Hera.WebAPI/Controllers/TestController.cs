using Microsoft.AspNetCore.Mvc;

namespace Hera.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET: api/<testcontroller> 
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Test api");
        }
    }

}
