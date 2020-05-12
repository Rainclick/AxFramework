using System.Collections.Generic;
using FluentValidation;

namespace Entities.Framework
{
    public class AxGroup : BaseEntity
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserGroup> GroupUsers { get; set; }
    }

    public class AxGroupValidator : AbstractValidator<AxGroup> { }
}
