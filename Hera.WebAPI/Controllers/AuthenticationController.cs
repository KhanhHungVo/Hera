using Hera.Common.Core;
using Hera.Common.WebAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hera.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : HeraBaseController
    {
        private readonly ILogger _logger;
        private readonly IHeraSecurity _heraSecurity;
        private readonly JwtTokenDescriptorOptions _tokenDescriptorOptions;

        public AuthenticationController(
            ILogger logger,
            IHeraSecurity heraSecurity,
            IOptions<JwtTokenDescriptorOptions> tokenDescriptorOptions
        ) : base()
        {
            _logger = logger;
            _heraSecurity = heraSecurity;
            _tokenDescriptorOptions = tokenDescriptorOptions.Value;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            byte[] encodedBytes= _heraSecurity.EncryptAes(password);
            string base64Password = Convert.ToBase64String(encodedBytes);

            // password HERABi48!
            if (!username.Contains("trietnguyen") || !base64Password.Contains("u2fAzCwEy5rnCJBw7dlCSQ=="))
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenDescriptorOptions.OAuthSignatureKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "USER")
                }),
                Issuer = _tokenDescriptorOptions.Issuer,
                Audience = _tokenDescriptorOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_tokenDescriptorOptions.AccessTokenExpiredTimeInMinute),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            return Ok(accessToken);
        }
    }
}
