using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common;
using Entities.Framework;
using WebFramework.Api;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace API.Models
{
    public class AddressDto : BaseDto<AddressDto, Address, int>, IValidatableObject
    {
        public string Content { get; set; }
        public int GeoId { get; set; }
        public string GeoTitle { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsMainAddress { get; set; }
        public int UserId { get; set; }
        public AddressType Type { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Content))
                yield return new ValidationResult("محتوای آدرس نباید خالی باشد", new[] { nameof(Content) });

            if (GeoId <= 0)
                yield return new ValidationResult("شهر آدرس نباید خالی باشد", new[] { nameof(Content) });
        }

        public override void CustomMappings(IMappingExpression<Address, AddressDto> mapping)
        {
            mapping.ForMember(
                dest => dest.GeoTitle,
                config => config.MapFrom(src => $"{src.Geo.Title}")
            );
        }


    }
}
