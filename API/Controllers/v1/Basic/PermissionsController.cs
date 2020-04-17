using System.Collections.Generic;
using System.Linq;
using API.Models;
using AutoMapper.QueryableExtensions;
using Common;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Basic
{
    [ApiVersion("1")]
    public class PermissionsController : BaseController
    {
        private readonly IBaseRepository<Permission> _repository;
        private readonly IBaseRepository<Menu> _menuRepository;

        public PermissionsController(IBaseRepository<Permission> repository, IBaseRepository<Menu> menuRepository)
        {
            _repository = repository;
            _menuRepository = menuRepository;
        }

        [HttpGet("[action]")]
        [AxAuthorize(StateType = StateType.Ignore, ShowInMenu = true, AxOp = AxOp.PermissionTree, Order = 0)]
        public ApiResult<dynamic> GetTree()
        {
            var permissions = _menuRepository.GetAll(x => x.Active)
                .Include(x => x.Children).ThenInclude(x => x.Children).ThenInclude(x => x.Children)
                .ThenInclude(x => x.Children);
            var newData = SerializeData(permissions.Where(x => x.ParentId == null).ToList(), 0);
            return Ok(newData);
        }

        private ICollection<dynamic> SerializeData(List<Menu> data, int userId)
        {
            //var keys = ((HashSet<string>)Session["UserPermissions"]).ToList();
            return data?.Where(x => x.Active).Select(x => new
            {
                id = x.Key,
                text = x.Title,
                children = x.Children != null && x.Children.Any() ? SerializeData(x.Children.ToList(), userId) : null
            }).ToArray();
        }
    }
}