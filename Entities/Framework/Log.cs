using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Framework
{
    public class Log : BaseEntity<long>
    {
        [Required]
        public DateTime Logged { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public string Message { get; set; }
        public string UserName { get; set; }
        public string ServerName { get; set; }
        public string Port { get; set; }
        public string Url { get; set; }
        public string ServerAddress { get; set; }
        public string RemoteAddress { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
