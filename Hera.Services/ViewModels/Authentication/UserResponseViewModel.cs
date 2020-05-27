﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Hera.Services.ViewModels.Authentication
{
    public class UserResponseViewModel
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public string Username { get; set; }
        public float Band { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool Onboarding { get; set; }
    }
}
