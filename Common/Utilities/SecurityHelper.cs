using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public static class SecurityHelper
    {
        public static string GetSha256Hash(string input)
        {
            //using (var sha256 = new SHA256CryptoServiceProvider())
            using (var sha256 = SHA256.Create())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = sha256.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
                //return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
            }
        }

        public static string GetIp(this IPAddress ipAddress)
        {
            var ip = ipAddress?.ToString();
            return ip;
        }

        public static string GetDeviceName(this IPAddress ipAddress)
        {
            try
            {
                var getIpHost = Dns.GetHostEntry(ipAddress);
                var compName = getIpHost.HostName.Split('.');
                return compName.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
