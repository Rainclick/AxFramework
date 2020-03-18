using System.Linq;
using System.Net;

namespace Common.Utilities
{
    public class CompNameHelper
    {
        public static string DetermineCompName(string ip)
        {
            try
            {
                var myIp = IPAddress.Parse(ip);
                var getIpHost = Dns.GetHostEntry(myIp);
                var compName = getIpHost.HostName.Split('.').ToList();
                return compName.FirstOrDefault();
            }
            catch
            {
                return null;
            }

        }
    }
}
