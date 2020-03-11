using System;
using Dapper.Contrib.Extensions;

namespace API.Models
{
    [Table("AxUserLoginLogs")]
    public class AxUserLoginLog
    {
        [ExplicitKey]
        public long Id { get; set; }
        public long? UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Ip { get; set; }
        public string Device { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
    }
}
