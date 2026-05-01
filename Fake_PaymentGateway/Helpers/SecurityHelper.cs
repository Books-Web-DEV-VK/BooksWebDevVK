using System.Text;

namespace Fake_PaymentGateway.Helpers
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
