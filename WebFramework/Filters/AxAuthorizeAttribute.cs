using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Exception;
using Common.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace WebFramework.Filters
{
    public class AxAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public AxOp AxOp { get; set; }
        public AxOp ParentAxOp => this.AxOp.GetAxParent();
        public bool ShowInMenu { get; }

        public StateType StateType { get; set; }

        public AxAuthorizeAttribute(AxOp axOp, bool showInMenu = false)
        {
            AxOp = axOp;
            ShowInMenu = showInMenu;
        }

        public AxAuthorizeAttribute()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var context = filterContext.HttpContext;
            var memoryCache = filterContext.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var userId = context.User.Identity.GetUserId<int>();
            if (StateType == StateType.OnlyToken && userId <= 0)
                throw new AppException(ApiResultStatusCode.UnAuthorized, "شما برای دسترسی به منابع مرود نظر احراز هویت نشده اید");

            if (StateType == StateType.Authorized)
            {
                var keys = memoryCache.Get<HashSet<string>>("user" + userId);
                var haveAccess = keys.Any(x => x.ToLower() == AxOp.GetAxKey());

                if (!haveAccess)
                {
                    if (StateType == StateType.CheckParent)
                    {
                        var haveAccessToParent = keys.Any(key => key == ParentAxOp.GetAxKey());
                        if (!haveAccessToParent)
                        {
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "شما دسترسی برای عملیات درخواست شده را ندارید");
                        }
                    }
                    else
                        throw new AppException(ApiResultStatusCode.UnAuthorized, "شما دسترسی برای عملیات درخواست شده را ندارید");
                }
            }
        }
    }
}
