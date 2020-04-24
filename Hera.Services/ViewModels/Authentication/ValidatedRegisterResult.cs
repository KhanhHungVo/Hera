using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Services.ViewModels.Authentication
{
    public class ValidatedRegisterResult
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }
    }
}
