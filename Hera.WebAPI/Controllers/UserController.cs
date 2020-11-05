using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.ViewModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Serilog;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Hera.WebAPI.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UserController: HeraBaseController
    {
        private readonly IUserService _userService;
        private readonly IHeraSecurity _heraSecurity;
        public UserController(
            IHttpContextAccessor httpContextAccessor,
            ILogger logger,
            IHeraSecurity heraSecurity,
            IUserService userService
        ) : base(httpContextAccessor)
        {
            _heraSecurity = heraSecurity;
            _userService = userService;
        }

        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userDataClaim = User.Claims.FirstOrDefault(c => c.Type == HeraConstants.CLAIM_TYPE_USER_DATA)?.Value;

            var userCredentials = JsonConvert.DeserializeObject<UserCredentials>(userDataClaim);
            var user = await _userService.GetByEmail(userCredentials.EmailAddress);
            return HeraOk(user);
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
            var user = await _userService.GetAsync(id.ToString());
            return HeraOk(user);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GeByEmailAsync(string email)
        {
            var user = await _userService.GetByEmail(email);
            return HeraOk(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }
            await _userService.UpdateAsync(model);
            return HeraCreated(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id.ToString());

            return HeraOk();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterViewModel model)
        {
            string hashedPassword = _heraSecurity.EncryptAes(model.Password);

            model.Password = hashedPassword;
            var errorMessage = await _userService.ValidateUserDefinedRules(model);
            if (errorMessage != null)
            {
                return HeraBadRequest(errorMessage);
            }
            var newUser = await _userService.CreateAsync(model);

            return HeraCreated(newUser);
        }

    }
}
