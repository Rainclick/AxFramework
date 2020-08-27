using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Framework;
using Entities.Tracking;
using WebFramework.Api;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace API.Models.Tracking
{
    public class MachineDto : BaseDto<MachineDto, Machine, int>, IValidatableObject
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string OperationStationName { get; set; }
        public int OperationStationId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("نام نباید خالی باشد", new[] { nameof(Name) });
        }
        public override void CustomMappings(IMappingExpression<Machine, MachineDto> mapping)
        {
            mapping.ForMember(
                dest => dest.OperationStationName,
                config => config.MapFrom(src => $"{src.OperationStation.Name} ")
            );
        }
    }
}
