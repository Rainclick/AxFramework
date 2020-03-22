using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Framework;
using WebFramework.Api;

namespace API.Models
{
    public class ConfigDataDto : BaseDto<ConfigDataDto, ConfigData, int>
    {
        public string OrganizationName { get; set; }
        public string VersionName { get; set; }
        public bool Active { get; set; }
    }
}
