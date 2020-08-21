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
    public class MachinesController : BaseController
    {
        private readonly IBaseRepository<Machine> _repository;

        public MachinesController(IBaseRepository<Machine> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.MachineList)]
        public virtual ApiResult<IQueryable<MachineDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<Machine>();
            var data = _repository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<MachineDto>();
            return Ok(data);
        }

        [HttpGet("{machineId}")]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.MachineItem)]
        public virtual ApiResult<MachineDto> Get(int machineId, int userId)
        {
            var machineDto = _repository.GetAll(x => x.Id == machineId).ProjectTo<MachineDto>().SingleOrDefault();
            return Ok(machineDto);
        }

        [HttpPost]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.MachineInsert)]
        public virtual async Task<ApiResult<MachineDto>> Create(MachineDto dto, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(dto.ToEntity(), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<MachineDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }
        [HttpPut]
        [AxAuthorize(StateType = StateType.Authorized, Order = 2, AxOp = AxOp.MachineUpdate)]
        public virtual async Task<ApiResult<MachineDto>> Update(MachineDto dto, CancellationToken cancellationToken)
        {
            var machine = await _repository.GetFirstAsync(x => x.Id == dto.Id, cancellationToken);
            if (machine == null)
                throw new NotFoundException("ماشینی یافت نشد");

            await _repository.UpdateAsync(dto.ToEntity(machine), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<MachineDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }

        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.MachineDelete, Order = 3)]
        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstAsync(x => x.Id.Equals(id), cancellationToken);
            await _repository.DeleteAsync(model, cancellationToken);
            return Ok();
        }
    }
}
