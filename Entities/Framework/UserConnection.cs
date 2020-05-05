using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework
{
    public class UserConnection : BaseEntity
    {
        public string ConnectionId { get; set; }
        public string Ip { get; set; }
        public int UserId { get; set; }
        public int UserTokenId { get; set; }
        [ForeignKey("UserTokenId")]
        public UserToken UserToken{ get; set; }
    }

}
