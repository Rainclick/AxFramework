using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Framework
{
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType GenderType { get; set; }
        public DateTime BirthDate { get; set; }
        public string FullName => FirstName + " " + LastName;
        public DateTimeOffset? LastLoginDate { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }

    public enum GenderType
    {
        Female = 0,
        Male = 1,
        None = 2
    }

}
