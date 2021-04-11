using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Hera.Common.ViewModels.Authentication
{
    public class UserLoginViewModel
    {
        [Required(AllowEmptyStrings=false)]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
