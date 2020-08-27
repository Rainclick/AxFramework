using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Tracking;
using WebFramework.Api;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace API.Models.Tracking
{
    public class PersonnelDto : BaseDto<PersonnelDto, Personnel, int>, IValidatableObject
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public string FactoryName { get; set; }
        public int FactoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Code))
                yield return new ValidationResult("کد نباید خالی باشد", new[] { nameof(Code) });
        }
        public override void CustomMappings(IMappingExpression<Personnel, PersonnelDto> mapping)
        {
            mapping.ForMember(
                dest => dest.UserName,
                config => config.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}")
            );
            mapping.ForMember(
                dest => dest.FactoryName,
                config => config.MapFrom(src => $"{src.Factory.Name} ")
            );
        }

    }
}
