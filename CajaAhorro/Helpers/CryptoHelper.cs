
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;

using System.Text;

namespace Money_Box.Helpers
{
    public static class CryptoHelper
    {
        public static string SHA512(string str)
        {
            using var sha512 = System.Security.Cryptography.SHA512.Create();
            byte[] stream = sha512.ComputeHash(Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(stream).Replace("-", "").ToLowerInvariant();
        }
    }
}
