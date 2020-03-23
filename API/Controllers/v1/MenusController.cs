using System.Linq;
using API.Models;
using AutoMapper.QueryableExtensions;
using Common;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [AxAuthorize(StateType = StateType.OnlyToken)]
        public virtual ApiResult<IQueryable<MenuDto>> Get(int? parentId)
        {
            var menus = _repository.GetAll(x => x.ParentId == parentId).ProjectTo<MenuDto>();
            var data = _repository.GetAll(x => x.ParentId == parentId).ProjectTo<MenuDto>();
            //var query = menus    // your starting point - table in the "from" statement
            //    .Join(database.Post_Metas, // the source table of the inner join
            //        post => post.ID,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
            //        meta => meta.Post_ID,   // Select the foreign key (the second part of the "on" clause)
            //        (post, meta) => new { Post = post, Meta = meta }) // selection
            //    .Where(postAndMeta => postAndMeta.Post.ID == id);
            if (menus == null)
                return NotFound();
            return Ok(menus);
        }


    }
}