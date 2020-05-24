using Google.Apis.Auth;
using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.Models;
using Hera.Services.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
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
        public async Task<IActionResult> LoginWithGoogle([FromBody]SocialUserInfo model)
        {
            var payload = GoogleJsonWebSignature.ValidateAsync(model.TokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
            _logger.Information(payload.ExpirationTimeSeconds.ToString());
            var jwtToken = await _authenticationService.AuthenticateWithGoogle(payload);
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
