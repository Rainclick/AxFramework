using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace Entities.Framework
{
    public class UserGroup : BaseEntity
    {
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual AxGroup AxGroup { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
    public class UserGroupValidator : AbstractValidator<UserGroup> { }
}
