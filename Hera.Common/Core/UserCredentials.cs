using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Core
{
    public class UserCredentials
    {
        public string[] Roles { get; set; }
        public float Band { get; set; }
        public bool IsOnboarding { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public string MobilePhone { get; set; }
    }
}
