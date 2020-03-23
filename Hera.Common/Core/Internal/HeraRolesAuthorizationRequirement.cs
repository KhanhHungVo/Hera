using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Common.Core.Internal
{
    internal class HeraRolesAuthorizationRequirement : IAuthorizationRequirement
    {
        public IEnumerable<string> HeraRoles { get; set; }

        public HeraRolesAuthorizationRequirement(params string[] roles)
        {
            HeraRoles = roles;
        }
    }

    internal class HeraRolesAuthorizationRequirementHandler : AuthorizationHandler<HeraRolesAuthorizationRequirement>
    {
        public JwtTokenDescriptorOptions jwtTokenDescriptorOptions { get; set; }
        public HeraRolesAuthorizationRequirementHandler(IOptions<JwtTokenDescriptorOptions> tokenDescriptorOptions)
        {
            jwtTokenDescriptorOptions = tokenDescriptorOptions.Value;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HeraRolesAuthorizationRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.UserData &&
                                        c.Issuer == jwtTokenDescriptorOptions.Issuer))
            {
                return Task.CompletedTask;
            }

            var userDataClaim = context.User.FindFirst(c => c.Type == ClaimTypes.UserData &&
                                                        c.Issuer == jwtTokenDescriptorOptions.Issuer).Value;
            var userCredentials = JsonConvert.DeserializeObject<UserCredentials>(userDataClaim);

            foreach (var role in userCredentials.Roles)
            {
                if (requirement.HeraRoles.Any(heraRole => role.Equals(heraRole)))
                {
                    context.Succeed(requirement);
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
