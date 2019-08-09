using System.Linq;
using API.Models;
using AutoMapper.QueryableExtensions;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

namespace API.Controllers.v1
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class MenusController : BaseController
    {
        private readonly IBaseRepository<Menu> _repository;

        public MenusController(IBaseRepository<Menu> repository)
        {
            _repository = repository;
        }

        [HttpGet("{parentId?}")]
        public virtual ApiResult<IQueryable<MenuDto>> Get(int? parentId)
        {
            var menus = _repository.GetAll(x => x.ParentId == parentId).ProjectTo<MenuDto>();
            if (menus == null)
                return NotFound();
            return Ok(menus);
        }

    }
}