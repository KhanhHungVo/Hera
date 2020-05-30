using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.ViewModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;


namespace Hera.WebAPI.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UserController: HeraBaseController
    {
        private readonly IUserService _userService;
        public UserController(
            IHttpContextAccessor httpContextAccessor,
            ILogger logger,
            IUserService userService
        ) : base(httpContextAccessor)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserAsync()
        {
            var allUsers = await _userService.GetListAsync();
            return HeraOk(allUsers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = await _userService.GetById(id.ToString());
            return HeraOk(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }
            await _userService.UpdateUser(model);
            return HeraCreated(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id.ToString());

            return HeraOk();
        }

    }
}
