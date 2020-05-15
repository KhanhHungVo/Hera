using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Services.ViewModels.Authentication
{
    public class GoogleUserInfo
    {
        public string TokenId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }
}
