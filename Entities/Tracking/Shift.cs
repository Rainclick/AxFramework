using Entities.Framework;
using FluentValidation;

namespace Entities.Tracking
{
    public class Shift : BaseEntity
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class ShiftValidator : AbstractValidator<Shift>
    {
        public ShiftValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
