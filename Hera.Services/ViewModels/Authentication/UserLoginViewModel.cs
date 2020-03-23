﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hera.Services.ViewModels.Authentication
{
    public class UserLoginViewModel
    {
        [Required(AllowEmptyStrings=false)]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
    public class UserLoginRepsonseService
    {
        public string Username { get; set; }
        public float Band { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool Onboarding { get; set; }
    }
}
