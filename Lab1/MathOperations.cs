using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public static class MathOperations
    {
        public static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static byte[] sha256_byte(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            return crypto;
        }

        public static byte[] md5_byte(string randomString)
        {
            var crypt = System.Security.Cryptography.MD5.Create();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            return crypto;
        }
    }
}
