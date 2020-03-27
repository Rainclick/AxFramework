using System;
using Entities.Framework;
using WebFramework.Api;

namespace API.Models
{
    public class LogDto: BaseDto<LogDto, Log, int>
    {
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string ServerName { get; set; }
        public string Port { get; set; }
        public string Url { get; set; }
        public string ServerAddress { get; set; }
        public string RemoteAddress { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
    }
}
