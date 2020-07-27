using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Framework;
using WebFramework.Api;

namespace API.Models
{
    public class GeoDto : BaseDto<GeoDto, Geo, int>, IValidatableObject
    {
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Title))
                yield return new ValidationResult("عنوان موقعیت را وارد کنید", new[] { nameof(Title) });
        }
    }
}
