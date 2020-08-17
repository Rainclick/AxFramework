using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Tracking;
using WebFramework.Api;

namespace API.Models.Tracking
{
    public class FactoryDto: BaseDto<FactoryDto, Factory, int>, IValidatableObject
    {

        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("نام نباید خالی باشد", new[] { nameof(Name) });
        }
    }
}
