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
    public class ProductLinesController : BaseController
    {
        private readonly IBaseRepository<ProductLine> _repository;
        public ProductLinesController(IBaseRepository<ProductLine> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.ProductLineList)]
        public virtual ApiResult<IQueryable<ProductLineDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<ProductLine>();
            var data = _repository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<ProductLineDto>();
            return Ok(data);
        }

        [HttpGet("{ProductLineId}")]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.ProductLineItem)]
        public virtual ApiResult<ProductLineDto> Get(int productLineId, int userId)
        {
            var productLineDto = _repository.GetAll(x => x.Id == productLineId).ProjectTo<ProductLineDto>().SingleOrDefault();
            return Ok(productLineDto);
        }

        [HttpPost]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.TrackingInsert)]
        public virtual async Task<ApiResult<ProductLineDto>> Create(ProductLineDto dto, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(dto.ToEntity(), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<ProductLineDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }
        [HttpPut]
        [AxAuthorize(StateType = StateType.Authorized, Order = 2, AxOp = AxOp.TrackingUpdate)]
        public virtual async Task<ApiResult<ProductLineDto>> Update(ProductLineDto dto, CancellationToken cancellationToken)
        {
            var productLine = await _repository.GetFirstAsync(x => x.Id == dto.Id, cancellationToken);
            if (productLine == null)
                throw new NotFoundException("خط تولیدی یافت نشد");

            await _repository.UpdateAsync(dto.ToEntity(productLine), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<ProductLineDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
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
