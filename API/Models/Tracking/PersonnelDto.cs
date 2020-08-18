using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Entities.Tracking;
using WebFramework.Api;

namespace API.Models.Tracking
{
    public class PersonnelDto : BaseDto<PersonnelDto, Personnel, int>, IValidatableObject
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public int FactoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Code))
                yield return new ValidationResult("کد نباید خالی باشد", new[] { nameof(Code) });
        }
    }
}
