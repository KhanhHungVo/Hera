using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.ViewModels.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            ILogger logger,
            IHeraSecurity heraSecurity,
            IOptions<JwtTokenDescriptorOptions> tokenDescriptorOptions,
            IAuthenticationService authenticationService
        ) : base()
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

            return HeraOk(newUser);
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            byte[] encodedBytes= _heraSecurity.EncryptAes(password);
            string hashedPassword = Convert.ToBase64String(encodedBytes);

            var user = await _authenticationService.GetUserLogin(username, hashedPassword);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenDescriptorOptions.OAuthSignatureKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.MobilePhone, user.Telephone),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "USER")
                }),
                Issuer = _tokenDescriptorOptions.Issuer,
                Audience = _tokenDescriptorOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_tokenDescriptorOptions.AccessTokenExpiredTimeInMinute),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            return HeraOk(accessToken);
        }
    }
}
