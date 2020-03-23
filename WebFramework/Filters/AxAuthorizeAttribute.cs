using Common;
using Common.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebFramework.Filters
{
    public class AxAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public AxOp AxOp { get; set; }
        public AxOp ParentAxOp { get; set; }
        public bool ShowInMenu { get; }

        public StateType StateType { get; set; }

        public AxAuthorizeAttribute(AxOp axOp, AxOp parentAxOp, bool showInMenu = false)
        {
            AxOp = axOp;
            ParentAxOp = parentAxOp;
            ShowInMenu = showInMenu;
        }

        public AxAuthorizeAttribute(AxOp axOp, bool showInMenu = false)
        {
            AxOp = axOp;
            ShowInMenu  = showInMenu;
        }

        public AxAuthorizeAttribute()
        {
            
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var context = filterContext.HttpContext;
            //var userRepository = filterContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var userId = context.User.Identity.GetUserId<int>();
            //var keys = UserPermissionManager.GetKeys(userId);
          
            //if (keys != null && !haveAccess.Value)
            //{
            //}

        }
    }
}
