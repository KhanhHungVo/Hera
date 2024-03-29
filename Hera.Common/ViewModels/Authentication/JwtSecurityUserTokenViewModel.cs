﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Hera.Common.ViewModels.Authentication
{
    public class JwtSecurityUserTokenViewModel
    {
        public JwtSecurityToken AccessToken { get; set; }
        public JwtSecurityToken RefreshToken { get; set; }
    }
}
