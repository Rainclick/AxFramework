using System;
using Dapper.Contrib.Extensions;

namespace Common
{
    [Table("AxToken")]
    public class TokenBag
    {
        [ExplicitKey]
        public long Id { get; set; }
        public string Token { get; set; }
        public long? UserId { get; set; }
        public string Device { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
