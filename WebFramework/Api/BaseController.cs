using Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Filters;

namespace WebFramework.Api
{
    [ApiController]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    public class BaseController : ControllerBase
    {
        //var menus = _repository.Run("select * from users").Where(x => x.ParentId == parentId).ProjectTo<MenuDto>();
        //public UserRepository UserRepository { get; set; } => todo Amir: property injection
        public bool UserIsAuthenticated => HttpContext.User.Identity.IsAuthenticated;
        public int UserId => HttpContext.User.Identity.GetUserId<int>();
        public string ClientId => HttpContext.User.Identity.GetClientId();
    }
}
