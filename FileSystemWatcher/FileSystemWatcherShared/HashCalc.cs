using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace FSW
{
    public static class HashCalc
    {
        public enum HashType { MD5, SHA1, SHA256 };

        public static string GenerateHash(HashType hashType, string key, string value)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            byte[] valBytes = Encoding.ASCII.GetBytes(value);

            return GenerateHash(hashType, keyBytes, valBytes);
        }

        public static string GenerateHash(HashType hashType, string key, byte[] valBytes)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);

            return GenerateHash(hashType, keyBytes, valBytes);
        }

        public static string GenerateHash(HashType hashType, byte[] keyBytes, byte[] valBytes)
        {
            byte[] resBytes = new byte[keyBytes.Length + valBytes.Length];

            keyBytes.CopyTo(resBytes, 0);
            valBytes.CopyTo(resBytes, keyBytes.Length);

            byte[] hashBytes = null;

            switch (hashType)
            {
                case HashType.MD5:
                    hashBytes = MD5.Create().ComputeHash(resBytes);
                    break;
                case HashType.SHA1:
                    hashBytes = SHA1.Create().ComputeHash(resBytes);
                    break;
                case HashType.SHA256:
                    hashBytes = SHA256.Create().ComputeHash(resBytes);
                    break;
            }

            return Convert.ToBase64String(hashBytes);
        }
    }
}
