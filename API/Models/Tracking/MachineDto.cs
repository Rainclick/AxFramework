using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Entities.Tracking;
using WebFramework.Api;

namespace API.Models.Tracking
{
    public class MachineDto : BaseDto<MachineDto, Machine, int>, IValidatableObject
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int OperationStationId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("نام نباید خالی باشد", new[] { nameof(Name) });
        }
    }
}
