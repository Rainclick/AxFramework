using System.Collections.Generic;
using System.Linq;
using API.Models;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Utilities;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1
{
    /// <summary>
    /// Fetch Menus or Systems for user 
    /// </summary>
    [ApiVersion("1")]
    public class MenusController : BaseController
    {
        private readonly IBaseRepository<Menu> _repository;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="memoryCache"></param>
        public MenusController(IBaseRepository<Menu> repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Get Systems And Menus for Signed User
        /// </summary>
        /// <param name="parentId">Parent node Id for fetching them children</param>
        /// <returns></returns>
        [HttpGet("{parentId?}")]
        [AxAuthorize(StateType = StateType.OnlyToken)]
        public virtual ApiResult<IQueryable<MenuDto>> Get(int? parentId)
        {
            var userId = User.Identity.GetUserId<int>();
            var menus = _repository.GetAll(x => x.ParentId == parentId).Include(x => x.Children).ProjectTo<MenuDto>();
            var keys = _memoryCache.Get<HashSet<string>>("user" + userId);
            if (keys == null)
                return NotFound();
            return Ok(menus);
        }


    }
}