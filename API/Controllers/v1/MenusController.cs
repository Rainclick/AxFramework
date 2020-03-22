using System.Linq;
using API.Models;
using AutoMapper.QueryableExtensions;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

namespace API.Controllers.v1
{
    /// <summary>
    /// Fetch Menus or Systems for user 
    /// </summary>
    [ApiVersion("1")]
    public class MenusController : BaseController
    {
        private readonly IBaseRepository<Menu> _repository;

        public MenusController(IBaseRepository<Menu> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get Systems And Menus for Signed User
        /// </summary>
        /// <param name="parentId">Parent node Id for fetching them children</param>
        /// <returns></returns>
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