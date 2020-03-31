using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public static class SecurityHelper
    {
        public static string GetSha256Hash(this string input)
        {
            //using (var sha256 = new SHA256CryptoServiceProvider())
            using (var sha256 = SHA256.Create())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = sha256.ComputeHash(byteValue);
                //return Convert.ToBase64String(byteHash);
                return BitConverter.ToString(byteHash).ToUpper();
            }
        }

        public static string Hash(this string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hash).ToUpper();
        }


    }
}
