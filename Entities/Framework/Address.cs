using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace Entities.Framework
{
    public class Address : BaseEntity
    {
        [Required]
        public string Content { get; set; }
        public int GeoId { get; set; }
        [ForeignKey("GeoId")]
        public Geo Geo { get; set; }
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
        [Display(Name = "خانه")]
        Home = 0,
        [Display(Name = "محل کار")]
        Work = 1,
        [Display(Name = "سایر")]
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
