using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            var user = await _authenticationService.GetUserLogin(model.Username, hashedPassword);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenDescriptorOptions.OAuthSignatureKey);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.MobilePhone, user.Telephone),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(new UserCredentials {
                    // HeraConstants.CLAIM_TYPE_ROLES
                    Roles = new string[] { HeraConstants.CLAIM_HERA_USER, "Student" },
                    Band = user.Band,
                    IsOnboarding = user.Onboarding,
                }, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() })),
            };

            int originalClaimsSize = claims.Length;
            Array.Resize(ref claims, originalClaimsSize + 1);

            var securityToken = new JwtSecurityToken(
                issuer: _tokenDescriptorOptions.Issuer,
                audience: _tokenDescriptorOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenDescriptorOptions.AccessTokenExpiredTimeInMinute),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            var accessToken = tokenHandler.WriteToken(securityToken);
            return HeraOk(accessToken);
        }
    }
}
