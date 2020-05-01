using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Framework
{
    public class UserConnection : BaseEntity
    {
        public string ConnectionId { get; set; }
        public bool Active { get; set; }
        public string Ip { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }

}
