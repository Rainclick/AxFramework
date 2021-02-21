using System.Collections.Generic;
using System.Linq;
using API.Models;
using Common;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebFramework.Api;
using WebFramework.Filters;
using WebFramework.UserData;

namespace API.Controllers.v1.Basic
{
    [ApiVersion("1")]
    public class PermissionsController : BaseController
    {
        private readonly IBaseRepository<Permission> _repository;
        private readonly IBaseRepository<Menu> _menuRepository;
        private readonly IBaseRepository<UserGroup> _userGroupRepository;
        private readonly IMemoryCache _memoryCache;

        public PermissionsController(IBaseRepository<Permission> repository, IBaseRepository<Menu> menuRepository, IMemoryCache memoryCache, IBaseRepository<UserGroup> userGroupRepository)
        {
            _repository = repository;
            _menuRepository = menuRepository;
            _memoryCache = memoryCache;
            _userGroupRepository = userGroupRepository;
        }

        [HttpGet("[action]/{id}")]
        [AxAuthorize(StateType = StateType.Authorized, ShowInMenu = true, AxOp = AxOp.PermissionTree, Order = 2)]
        public ApiResult<dynamic> GetTree(int id)
        {
            var permissions = _menuRepository.GetAll(x => x.Active)
                .Include(x => x.Children).ThenInclude(x => x.Children).ThenInclude(x => x.Children)
                .ThenInclude(x => x.Children);
            var newData = SerializeData(permissions.Where(x => x.ParentId == null).ToList(), id);
            return Ok(newData);
        }

        private ICollection<dynamic> SerializeData(List<Menu> data, int userId)
        {
            userId = userId == 0 ? UserId : userId;
            var keys = _memoryCache.GetOrCreate("user" + userId, entry => PermissionHelper.GetKeysFromDb(_repository, _userGroupRepository, userId));
            return data?.Where(x => x.Active).Select(x => new
            {
                x.Key,
                x.Title,
                Children = x.Children != null && x.Children.Any() ? SerializeData(x.Children.ToList(), userId) : null,
                IsLeaf = x.Children == null || x.Children.Count == 0,
                @checked = keys.Any(t => t == x.Key) ? true : (bool?)null,
            }).ToArray();
        }


        [HttpPost]
        [AxAuthorize(StateType = StateType.Authorized, ShowInMenu = false, AxOp = AxOp.PermissionTreeSave, Order = 0)]
        public ApiResult<dynamic> Save(UgType ugType, int id, List<string> data)
        {
            return Ok();
        }

    }
}