using Hera.Common.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hera.Common.WebAPI
{
    internal class EncryptionConfiguration
    {
        public string AesPrivateKey { get; set; }
        public string AesIVKey { get; set; }
    }

    public class HeraSecurity : IHeraSecurity
    {
        private static EncryptionConfiguration _encryptionConfiguration;

        public HeraSecurity(IConfiguration configuration)
        {
            _encryptionConfiguration = new EncryptionConfiguration();
            configuration.GetSection(HeraConstants.APP_SETTING__ENCRYPTION).Bind(_encryptionConfiguration);
        }

        public string EncryptAes(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            byte[] encrypted;
            byte[] keyBytes = ASCIIEncoding.ASCII.GetBytes(_encryptionConfiguration.AesPrivateKey);
            byte[] ivBytes = ASCIIEncoding.ASCII.GetBytes(_encryptionConfiguration.AesIVKey);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                // Create a encrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(source);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        public string DecryptAes(byte[] encryptedBytes)
        {
            if (encryptedBytes == null || encryptedBytes.Length <= 0)
            {
                throw new ArgumentNullException(nameof(encryptedBytes));
            }

            string source;
            byte[] keyBytes = ASCIIEncoding.ASCII.GetBytes(_encryptionConfiguration.AesPrivateKey);
            byte[] ivBytes = ASCIIEncoding.ASCII.GetBytes(_encryptionConfiguration.AesIVKey);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            source = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return source;
        }
    }
}
