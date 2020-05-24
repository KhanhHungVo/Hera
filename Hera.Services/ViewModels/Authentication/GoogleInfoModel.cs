using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Services.ViewModels.Authentication
{
    public class SocialUserInfo
    {
        public string UserId { get; set; }
        public string TokenId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
    }
}
