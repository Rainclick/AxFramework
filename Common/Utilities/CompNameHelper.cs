using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Common.Utilities
{
    public class CompNameHelper
    {
        public static string DetermineCompName(string ip)
        {
            var myIp = IPAddress.Parse(ip);
            var getIpHost = Dns.GetHostEntry(myIp);
            List<string> compName = getIpHost.HostName.Split('.').ToList();
            return compName.FirstOrDefault();
        }
    }
}
