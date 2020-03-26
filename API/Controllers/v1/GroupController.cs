using System.Threading;
using API.Models;
using AutoMapper.QueryableExtensions;
using Common;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1
{
    /// <summary>
    /// Group Methods
    /// </summary>
    [ApiVersion("1")]
    public class GroupController : BaseController
    {
        private readonly IBaseRepository<AxGroup> _groupRepository;

        public GroupController(IBaseRepository<AxGroup> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, Order = 0, AxOp = AxOp.GroupList, ShowInMenu = true)]
        public ApiResult<AxGroupDto> Get(CancellationToken cancellationToken)
        {
            var groups = _groupRepository.GetAll().ProjectTo<AxGroupDto>();
            return Ok(groups);
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.GroupList, ShowInMenu = true)]
        public ApiResult<AxGroupDto> Get(int id, CancellationToken cancellationToken)
        {
            var group = _groupRepository.GetAll(x => x.Id == id).ProjectTo<AxGroupDto>().FirstOrDefaultAsync(cancellationToken);
            return Ok(group);
        }

    }
}