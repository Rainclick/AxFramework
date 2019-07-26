using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserDto : IValidatableObject
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName != null && UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("نام کاربری نمیتواند Test باشد", new[] { nameof(UserName) });
            if (Password.Equals("123456"))
                yield return new ValidationResult("رمز عبور نمیتواند 123456 باشد", new[] { nameof(Password) });
        }
    }
}
