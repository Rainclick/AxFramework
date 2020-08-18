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
    public class OperationStationDto : BaseDto<OperationStationDto, OperationStation, int>, IValidatableObject
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int ProductLineId { get; set; }
        public int Order { get; set; }
        public float Vas { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("نام نباید خالی باشد", new[] { nameof(Name) });
        }
    }
}
