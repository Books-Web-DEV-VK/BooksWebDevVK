using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.Utility
{
    public class SecurityHelper
    {
        public static string GenerateSignature(string data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var hmac = new System.Security.Cryptography.HMACSHA256(keyBytes);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var hash = hmac.ComputeHash(dataBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
