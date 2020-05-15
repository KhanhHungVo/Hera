using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Core
{
    public interface IHeraSecurity
    {
        string EncryptAes(string source);
        string DecryptAes(byte[] encryptedBytes);
    }
}
