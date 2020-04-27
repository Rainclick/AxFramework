using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Framework
{
    public class Log : BaseEntity
    {
        [Required]
        public DateTime Logged { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public string Message { get; set; }
        public string UserName { get; set; }
        public string ServerName { get; set; }
        public string Url { get; set; }
        public string ServerAddress { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
        public string Ip { get; set; }
        public string Type { get; set; }
        public string UserAgent { get; set; }
    }
}
