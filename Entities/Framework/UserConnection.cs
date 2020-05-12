using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace Entities.Framework
{
    public class UserConnection : BaseEntity
    {
        public string ConnectionId { get; set; }
        public string Ip { get; set; }
        public int UserId { get; set; }
        public int UserTokenId { get; set; }
        [ForeignKey("UserTokenId")]
        public UserToken UserToken { get; set; }
    }


    public class UserConnectionValidator : AbstractValidator<UserConnection> { }

}
