using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Entities.Tracking;
using WebFramework.Api;

namespace API.Models.Tracking
{
    public class ShiftDto : BaseDto<ShiftDto, Shift, int>, IValidatableObject
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("نام نباید خالی باشد", new[] { nameof(Name) });
        }
    }
}
