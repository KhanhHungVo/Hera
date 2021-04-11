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
            if(data is null)
            {
                return NoContent();
            }
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
        public IActionResult HeraNotFound(string message = null)
        {
            return NotFound(message);
        }

        [NonAction]
        public IActionResult HeraBadRequest(string message = null)
        {
            string messages = string.Join("; ", ModelState.Values
                                                          .SelectMany(x => x.Errors)
                                                          .Select(x => x.ErrorMessage));
            if (String.IsNullOrEmpty(message))
            {
                return BadRequest(messages);
            }

            return BadRequest($"{message}; {messages}");
            
        }

        protected void SetUserCredentialsFromAccessToken(string accessToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(accessToken);
            var userDataClaim = jwtSecurityToken.Claims.Where(claim => claim.Type == HeraConstants.CLAIM_TYPE_USER_DATA).FirstOrDefault();
            if (userDataClaim == null)
            {
                return;
            }

            UserCredentials = JsonConvert.DeserializeObject<UserCredentials>(userDataClaim.Value);
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

            // maybe token is fake, So we put this code into try catch block
            try
            {
                SetUserCredentialsFromAccessToken(authorizationHeader.Replace("Bearer ", string.Empty));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
