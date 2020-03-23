using Hera.Common.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Hera.Common.WebAPI
{
    public abstract class HeraBaseController : ControllerBase
    {
        protected UserCredentials UserCredentials;

        public HeraBaseController(IHttpContextAccessor httpContextAccessor)
        {
            SetUserCredentials(httpContextAccessor);
        }

        protected object EncryptRequest(object data)
        {
            return data;
        }

        [NonAction]
        public IActionResult HeraOk()
        {
            return Ok();
        }

        [NonAction]
        public IActionResult HeraOk(object data)
        {
            return Ok(data);
        }

        [NonAction]
        public IActionResult HeraNoContent()
        {
            return NoContent();
        }

        [NonAction]
        public IActionResult HeraCreated(object data = null)
        {
            return Created("", data);
        }

        [NonAction]
        public IActionResult HeraBadRequest(object error)
        {
            return BadRequest(error);
        }

        [NonAction]
        public IActionResult HeraBadRequest()
        {
            string messages = string.Join("; ", ModelState.Values
                                                          .SelectMany(x => x.Errors)
                                                          .Select(x => x.ErrorMessage));
            return BadRequest(messages);
        }

        private void SetUserCredentials(IHttpContextAccessor httpContextAccessor)
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                return;
            }

            if (!authorizationHeader.StartsWith("Bearer "))
            {
                return;
            }

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var accessToken = jwtSecurityTokenHandler.ReadJwtToken(authorizationHeader.Replace("Bearer ", string.Empty));
            var userDataClaim = accessToken.Claims.Where(claim => claim.Type == ClaimTypes.UserData).FirstOrDefault();
            if (userDataClaim == null)
            {
                return;
            }

            UserCredentials = JsonConvert.DeserializeObject<UserCredentials>(userDataClaim.Value);
        }
    }
}
