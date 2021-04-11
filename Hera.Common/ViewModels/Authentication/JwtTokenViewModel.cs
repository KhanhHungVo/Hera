using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hera.Common.ViewModels.Authentication
{
    public class JwtTokenViewModel
    {
        [Required(AllowEmptyStrings=false)]
        public string AccessToken { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string RefreshToken { get; set; }
    }
}
