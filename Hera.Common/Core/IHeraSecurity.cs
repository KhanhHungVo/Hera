using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Core
{
    public interface IHeraSecurity
    {
        byte[] EncryptAes(string source);
        string DecryptAes(byte[] encryptedBytes);
    }
}
