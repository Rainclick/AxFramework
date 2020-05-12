using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace Entities.Framework
{
    public class Permission : BaseEntity
    {
        public bool Access { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User Group { get; set; }
        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual AxGroup User { get; set; }
    }

    public class PermissionValidator : AbstractValidator<Permission> { }
}
