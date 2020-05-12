using System;
using FluentValidation;

namespace Entities.Framework
{
    public class ConfigData : BaseEntity
    {
        public string OrganizationName { get; set; }
        public byte[] OrganizationLogo { get; set; }
        public string VersionName { get; set; }
        public DateTime? VersionDate { get; set; }
        public bool Active { get; set; }
    }

    public class ConfigDataValidator : AbstractValidator<ConfigData> { }
}
