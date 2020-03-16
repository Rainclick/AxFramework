using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Entities.Framework
{
    public class UserToken : BaseEntity
    {
        public string Token { get; set; }
        public bool Active { get; set; }
        public string DeviceName { get; set; }
        public string Ip { get; set; }
        public string Browser { get; set; }
        public string UserAgent { get; set; }
        public string ClientId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
