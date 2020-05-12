using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Entities.Framework
{
    public class Address : BaseEntity
    {
        [Required]
        public string Content { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsMainAddress { get; set; }
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }
        public AddressType AddressType { get; set; }
        public DateTime? ExpireDateTime { get; set; }

    }

    public enum AddressType
    {
        Home = 0,
        Work = 1,
        Other = 2
    }

    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
