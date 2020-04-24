using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Hera.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : HeraBaseController
    {
        private readonly ILogger _logger;
        private readonly IHeraSecurity _heraSecurity;
        private readonly JwtTokenDescriptorOptions _tokenDescriptorOptions;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(
            IHttpContextAccessor httpContextAccessor,
            ILogger logger,
            IHeraSecurity heraSecurity,
            IOptions<JwtTokenDescriptorOptions> tokenDescriptorOptions,
            IAuthenticationService authenticationService
        ) : base(httpContextAccessor)
        {
            _logger = logger;
            _heraSecurity = heraSecurity;
            _tokenDescriptorOptions = tokenDescriptorOptions.Value;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel userRegister)
        {
            byte[] encodedBytes = _heraSecurity.EncryptAes(userRegister.Password);
            string hashedPassword = Convert.ToBase64String(encodedBytes);

            userRegister.Password = hashedPassword;
            var newUser = await _authenticationService.Register(userRegister);

            return HeraCreated(newUser);
        }

        [HttpGet]
        [Route("validate-email")]
        public async Task<IActionResult> ValidateEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return HeraBadRequest(new ValidatedRegisterResult
                {
                    IsValid = false,
                    Message = "Email is empty",
                });
            }

            var result = await _authenticationService.ValidateEmail(email);
            if (!result.IsValid)
            {
                return HeraBadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("validate-telephone")]
        public async Task<IActionResult> ValidateTelephone(string telephone)
        {
            if (String.IsNullOrWhiteSpace(telephone))
            {
                return HeraBadRequest("Telephone is empty");
            }

            var result = await _authenticationService.ValidatePhoneNumber(telephone);
            if (!result.IsValid)
            {
                return HeraBadRequest(result.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }

            byte[] encodedBytes = _heraSecurity.EncryptAes(model.Password);
            string hashedPassword = Convert.ToBase64String(encodedBytes);

            var tokenResult = await _authenticationService.SignIn(model.Username, hashedPassword);
            if (tokenResult == null)
            {
                return HeraNoContent();
            }
            return HeraOk(tokenResult);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] JwtTokenViewModel jwtToken)
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }

            SetUserCredentialsFromAccessToken(jwtToken.AccessToken);

            var newJwtToken = await _authenticationService.AcquireNewToken(UserCredentials, jwtToken);

            if (newJwtToken == null)
            {
                return HeraNoContent();
            }

            return HeraOk(newJwtToken);
        }
    }
}
