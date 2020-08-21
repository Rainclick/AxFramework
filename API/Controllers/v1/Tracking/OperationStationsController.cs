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
    public class OperationStationsController : BaseController
    {
        private readonly IBaseRepository<OperationStation> _repository;

        public OperationStationsController(IBaseRepository<OperationStation> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.OperationStationList)]
        public virtual ApiResult<IQueryable<OperationStationDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<OperationStation>();
            var data = _repository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<OperationStationDto>();
            return Ok(data);
        }

        [HttpGet("{operationStationId}")]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.OperationStationItem)]
        public virtual ApiResult<OperationStationDto> Get(int operationStationId, int userId)
        {
            var operationStationDto = _repository.GetAll(x => x.Id == operationStationId).ProjectTo<OperationStationDto>().SingleOrDefault();
            return Ok(operationStationDto);
        }

        [HttpPost]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.OperationStationInsert)]
        public virtual async Task<ApiResult<OperationStationDto>> Create(OperationStationDto dto, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(dto.ToEntity(), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<OperationStationDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }
        [HttpPut]
        [AxAuthorize(StateType = StateType.Authorized, Order = 2, AxOp = AxOp.OperationStationUpdate)]
        public virtual async Task<ApiResult<OperationStationDto>> Update(OperationStationDto dto, CancellationToken cancellationToken)
        {
            var op = await _repository.GetFirstAsync(x => x.Id == dto.Id, cancellationToken);
            if (op == null)
                throw new NotFoundException("ایستگاه کاری یافت نشد");

            await _repository.UpdateAsync(dto.ToEntity(op), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<OperationStationDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }

        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.OperationStationDelete, Order = 3)]
        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstAsync(x => x.Id.Equals(id), cancellationToken);
            await _repository.DeleteAsync(model, cancellationToken);
            return Ok();
        }
    }
}
