using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Framework;
using WebFramework.Api;

namespace API.Models
{
    public class AddressDto : BaseDto<AddressDto, Address, int>, IValidatableObject
    {
        public string Content { get; set; }
        public string City { get; set; }
        public string UserFirstName { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Content))
                yield return new ValidationResult("محتوای آدرس نباید خالی باشد", new[] { nameof(Content) });
        }
    }
}
