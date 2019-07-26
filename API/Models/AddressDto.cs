using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class AddressDto : IValidatableObject
    {
        public string Content { get; set; }
        public string City { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Content))
                yield return new ValidationResult("محتوای آدرس نباید خالی باشد", new[] { nameof(Content) });
        }
    }
}
