using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.ViewModels.Authentication
{
    public class ValidatedRegisterResult
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }
    }
}
