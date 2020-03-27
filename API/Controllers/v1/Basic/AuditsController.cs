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

namespace API.Controllers.v1.Basic
{
    [ApiVersion("1")]
    public class AuditsController : BaseController
    {
        private readonly IBaseRepository<Log> _repository;

        public AuditsController(IBaseRepository<Log> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.LogList, ShowInMenu = true)]
        public virtual ApiResult<IQueryable<LogDto>> Get()
        {
            var data = _repository.GetAll().ProjectTo<LogDto>();
            return Ok(data);
        }

        [HttpGet]
        [Route("{id}")]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.LogItem)]
        public ApiResult<Log> Get(int id)
        {
            var user = _repository.GetAll(x => x.Id == id).FirstOrDefault();
            return Ok(user);
        }


        [HttpPost]
        [AxAuthorize(StateType = StateType.OnlyToken)]
        public virtual async Task<ApiResult<LogDto>> Create(Log dto, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(dto, cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<LogDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }
    }
}