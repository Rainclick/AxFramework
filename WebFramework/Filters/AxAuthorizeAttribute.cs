using System;
using System.Linq;
using Common.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using WebFramework.UserData;

namespace WebFramework.Filters
{
    public class AxAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public string Key { get; set; }

        public AxAuthorizeAttribute(string key)
        {
            Key = key;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var context = filterContext.HttpContext;
            //var userRepository = filterContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var userId = context.User.Identity.GetUserId<int>();
            var keys = UserPermissionManager.GetKeys(userId);
            var haveAccess = keys?.Any(x => string.Equals(x, Key, StringComparison.CurrentCultureIgnoreCase));
            if (keys != null && !haveAccess.Value)
            {
            }

        }
    }
}
