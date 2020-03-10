using System;
using Dapper.Contrib.Extensions;

namespace API.Models
{
    [Table("AxToken")]
    public class TokenBag
    {
        [ExplicitKey]
        public long Id { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Device { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
