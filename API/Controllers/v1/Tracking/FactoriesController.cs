using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models.Tracking;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exception;
using Data.Repositories;
using Entities.Framework.Reports;
using Entities.Tracking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;
using Common.Utilities;
namespace API.Controllers.v1.Tracking
{
    [ApiVersion("1")]
    public class FactoriesController : BaseController
    {
        private readonly IBaseRepository<Factory> _repository;

        public FactoriesController(IBaseRepository<Factory> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.TrackingFactoryList)]
        public virtual ApiResult<IQueryable<FactoryDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<Factory>();
            var data = _repository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<FactoryDto>();
            return Ok(data);
        }

        [HttpGet("{factoryId}")]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.FactoryItem)]
        public virtual ApiResult<FactoryDto> Get(int factoryId, int userId)
        {
            var factoryDto = _repository.GetAll(x => x.Id == factoryId).ProjectTo<FactoryDto>().SingleOrDefault();
            return Ok(factoryDto);
        }

        [HttpPost]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.FactoryInsert)]
        public virtual async Task<ApiResult<FactoryDto>> Create(FactoryDto dto, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(dto.ToEntity(), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<FactoryDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }
        [HttpPut]
        [AxAuthorize(StateType = StateType.Authorized, Order = 2, AxOp = AxOp.FactoryUpdate)]
        public virtual async Task<ApiResult<FactoryDto>> Update(FactoryDto dto, CancellationToken cancellationToken)
        {
            var factory = await _repository.GetFirstAsync(x => x.Id == dto.Id, cancellationToken);
            if (factory == null)
                throw new NotFoundException("کارخانه ای یافت نشد");

            await _repository.UpdateAsync(dto.ToEntity(factory), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<FactoryDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }

        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.FactoryDelete, Order = 3)]
        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstAsync(x => x.Id.Equals(id), cancellationToken);
            await _repository.DeleteAsync(model, cancellationToken);
            return Ok();
        }
    }
}
