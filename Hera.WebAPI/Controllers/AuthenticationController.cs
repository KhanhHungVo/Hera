using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Common.ViewModels.Authentication;
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
            string hashedPassword = _heraSecurity.EncryptAes(userRegister.Password);

            userRegister.Password = hashedPassword;
            var errorMessage = await _authenticationService.ValidateUserDefinedRules(userRegister);
            if(errorMessage != null)
            {
                return HeraBadRequest(errorMessage);
            }
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

            string hashedPassword = _heraSecurity.EncryptAes(model.Password);

            var tokenResult = await _authenticationService.SignIn(model.Username, hashedPassword);
            if (tokenResult is null)
            {
                return HeraNotFound("Username or password is not correct");
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

            return HeraOk(newJwtToken);
        }

        [HttpPost]
        [Route("sigin-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody]SocialUserInfo ggUserInfo)
        {
            if (string.IsNullOrEmpty(ggUserInfo.TokenId))
            {
                return HeraBadRequest("Token is null or empty");
            }
            var jwtToken = await _authenticationService.AuthenticateWithGoogle(ggUserInfo);
            return HeraOk(jwtToken);
        }

        [HttpPost]
        [Route("signin-facebook")]
        public async Task<IActionResult> LoginWithFaceBook([FromBody]SocialUserInfo fbUserInfo)
        {
            if (string.IsNullOrEmpty(fbUserInfo.TokenId))
            {
                return HeraBadRequest("Token is null or empty");
            }
            var jwtToken = await _authenticationService.AuthenticateWithFacebook(fbUserInfo);
            return HeraOk(jwtToken);
        }
    }
}
