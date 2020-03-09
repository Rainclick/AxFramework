using System;

namespace API.Models
{
    public class ConfigDto
    {
        public bool IsMandatory { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
        public string Description { get; set; }
        public string SplashText { get; set; }
        public string Link { get; set; }
        public DateTime DateTime { get; set; }
    }
}
