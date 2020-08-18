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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Tracking
{
    [ApiVersion("1")]
    public class ShiftsController : BaseController
    {
        private readonly IBaseRepository<Shift> _repository;

        public ShiftsController(IBaseRepository<Shift> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.ShiftList)]
        public virtual ApiResult<IQueryable<ShiftDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<Shift>();
            var data = _repository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<ShiftDto>();
            return Ok(data);
        }

        [HttpGet("{shiftId}")]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.ShiftItem)]
        public virtual ApiResult<ShiftDto> Get(int shiftId, int userId)
        {
            var ShiftDto = _repository.GetAll(x => x.Id == shiftId).ProjectTo<ShiftDto>().SingleOrDefault();
            return Ok(ShiftDto);
        }

        [HttpPost]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.TrackingInsert)]
        public virtual async Task<ApiResult<ShiftDto>> Create(ShiftDto dto, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(dto.ToEntity(), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<ShiftDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }
        [HttpPut]
        [AxAuthorize(StateType = StateType.Authorized, Order = 2, AxOp = AxOp.TrackingUpdate)]
        public virtual async Task<ApiResult<ShiftDto>> Update(ShiftDto dto, CancellationToken cancellationToken)
        {
            var shift = await _repository.GetFirstAsync(x => x.Id == dto.Id, cancellationToken);
            if (shift == null)
                throw new NotFoundException("شیفت مورد نظر یافت نشد");

            await _repository.UpdateAsync(dto.ToEntity(shift), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<ShiftDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }

        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.TrackingDelete, Order = 3)]
        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstAsync(x => x.Id.Equals(id), cancellationToken);
            await _repository.DeleteAsync(model, cancellationToken);
            return Ok();
        }
    }
}
