using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.CryptoService
{
    public interface ICryptoService
    {
        Task<string> makeAPICall();
    }
}
