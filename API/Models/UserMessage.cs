using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Utilities;
using Dapper.Contrib.Extensions;
using Entities.Framework;

namespace API.Models
{
    public class UserMessageDto 
    {
        [ExplicitKey]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public long? UserId { get; set; }
        public bool IsSeen { get; set; }
        public MsgType Type { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string DateString => InsertDateTime.ToPerDateTimeString();
        public DateTime? SeenDateTime { get; set; }
        public string SeenDevice{ get; set; }
    }

    public enum MsgType
    {
        OnlyText = 0
    }
}
