using System.Collections.Generic;
using System.Linq;

namespace API.Models
{
    public class UserInfo
    {
        public string UserDisplayName { get; set; }
        public int UnReedMsgCount { get; set; }
        public string OrganizationName { get; set; }    
        public string UserTheme { get; set; }
        public string DateTimeNow { get; set; }
        public string UserName { get; set; }
        public string VersionName { get; set; }
        public IQueryable<AxSystem> SystemsList { get; set; }
        public int? DefaultSystemId { get; set; }
        public string OrganizationLogo { get; set; }
        public string UserPicture { get; set; }
    }
}