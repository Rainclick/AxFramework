using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using AutoMapper.QueryableExtensions;
using Common;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;
using Common.Utilities;
using Entities.Framework.Reports;

namespace API.Controllers.v1.Basic
{
    /// <summary>
    /// Group Methods
    /// </summary>
    [ApiVersion("1")]
    public class GroupsController : BaseController
    {
        private readonly IBaseRepository<AxGroup> _groupRepository;

        public GroupsController(IBaseRepository<AxGroup> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, Order = 0, AxOp = AxOp.GroupList, ShowInMenu = true)]
        public ApiResult<IQueryable<AxGroupDto>> Get([FromQuery] DataRequest request, CancellationToken cancellationToken)
        {
            var predicate = request.GetFilter<AxGroup>();
            var groups = _groupRepository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<AxGroupDto>();
            Response.Headers.Add("X-Pagination", _groupRepository.Count(predicate).ToString());
            return Ok(groups);
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.GroupItem)]
        [Route("{id}")]
        public ApiResult<AxGroupDto> Get(int id, CancellationToken cancellationToken)
        {
            var group = _groupRepository.GetAll(x => x.Id == id).ProjectTo<AxGroupDto>().FirstOrDefaultAsync(cancellationToken);
            return Ok(group);
        }

    }
}