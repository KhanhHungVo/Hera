using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Core
{
    public class JwtTokenDescriptorOptions
    {
        public string OAuthSignatureKey { get; set; }
        public int AccessTokenExpiredTimeInMinute { get; set; }
        public int RefreshTokenExpiredTimeInMinute { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
