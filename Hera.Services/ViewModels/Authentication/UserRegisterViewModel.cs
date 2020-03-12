using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Services.ViewModels.Authentication
{
    public class UserRegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public float Band { get; set; }
        public short Age { get; set; }

        public UserRegisterViewModel WithoutPassword()
        {
            this.Password = this.ConfirmedPassword = "";
            return this;
        }
    }
}
