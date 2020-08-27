using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Entities.Tracking;
using WebFramework.Api;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace API.Models.Tracking
{
    public class ProductInstanceDto : BaseDto<ProductInstanceDto, ProductInstance, int>, IValidatableObject
    {

        public long Code { get; set; }
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Code < 0)
                yield return new ValidationResult("کد نباید خالی باشد", new[] { nameof(Code) });
        }

        public override void CustomMappings(IMappingExpression<ProductInstance, ProductInstanceDto> mapping)
        {
            mapping.ForMember(
                dest => dest.UserName,
                config => config.MapFrom(src => $"{src.Personnel.User.FirstName} {src.Personnel.User.LastName}")
            );
        }
    }
}
