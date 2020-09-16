using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models.Tracking;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exception;
using Common.Utilities;
using Data.Repositories;
using Entities.Framework.Reports;
using Entities.Tracking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Tracking
{
    [ApiVersion("1")]
    public class PersonnelsController : BaseController
    {
        private readonly IBaseRepository<Personnel> _repository;

        public PersonnelsController(IBaseRepository<Personnel> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.PersonnelList)]
        public virtual ApiResult<IQueryable<PersonnelDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<Personnel>();
            var data = _repository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<PersonnelDto>();
            Response.Headers.Add("X-Pagination", _repository.Count(predicate).ToString());
            return Ok(data);
        }

        [HttpGet("{personnelId}")]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.PersonnelItem)]
        public virtual ApiResult<PersonnelDto> Get(int personnelId, int userId)
        {
            var personnelDto = _repository.GetAll(x => x.Id == personnelId).ProjectTo<PersonnelDto>().SingleOrDefault();
            return Ok(personnelDto);
        }

        [HttpPost]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.PersonnelInsert)]
        public virtual async Task<ApiResult<PersonnelDto>> Create(PersonnelDto dto, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(dto.ToEntity(), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<PersonnelDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }
        [HttpPut]
        [AxAuthorize(StateType = StateType.Authorized, Order = 2, AxOp = AxOp.PersonnelUpdate)]
        public virtual async Task<ApiResult<PersonnelDto>> Update(PersonnelDto dto, CancellationToken cancellationToken)
        {
            var op = await _repository.GetFirstAsync(x => x.Id == dto.Id, cancellationToken);
            if (op == null)
                throw new NotFoundException("پرسنلی یافت نشد");

            await _repository.UpdateAsync(dto.ToEntity(op), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<PersonnelDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }

        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.PersonnelDelete, Order = 3)]
        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstAsync(x => x.Id.Equals(id), cancellationToken);
            await _repository.DeleteAsync(model, cancellationToken);
            return Ok();
        }
    }
}
