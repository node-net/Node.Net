using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Node.Net.Deprecated.Data.Security
{
    public static class SymmetricAlgorithmExtension
    {
        public static SymmetricAlgorithm GenerateKeyIV(SymmetricAlgorithm algorithm, string password, int keyBitLength)
        {
            var salt = new byte[] { 1, 2, 23, 234, 37, 48, 134, 63, 248, 4 };

            const int Iterations = 234;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                if (!algorithm.ValidKeySize(keyBitLength))
                    throw new InvalidOperationException("Invalid size key");

                algorithm.Key = rfc2898DeriveBytes.GetBytes(keyBitLength / 8);
                algorithm.IV = rfc2898DeriveBytes.GetBytes(algorithm.BlockSize / 8);
                return algorithm;
            }
        }

        public static byte[] Encrypt(SymmetricAlgorithm algorithm,byte[] data)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (CryptoStream encrypt = new CryptoStream(memory, algorithm.CreateEncryptor(algorithm.Key, algorithm.IV), CryptoStreamMode.Write))
                {
                    encrypt.Write(data, 0, data.Length);
                }
                return memory.ToArray();
            }
        }

        public static byte[] Decrypt(SymmetricAlgorithm algorithm,byte[] data)
        {
            using (MemoryStream memory = new MemoryStream(data))
            {
                using (CryptoStream decrypt = new CryptoStream(memory, algorithm.CreateDecryptor(algorithm.Key, algorithm.IV), CryptoStreamMode.Read))
                {
                    var result = new List<byte>();
                    var ibyte = decrypt.ReadByte();
                    while(ibyte > -1)
                    {
                        result.Add((byte)ibyte);
                        ibyte = decrypt.ReadByte();
                    }
                    return result.ToArray();
                }

            }
        }
    }
}
