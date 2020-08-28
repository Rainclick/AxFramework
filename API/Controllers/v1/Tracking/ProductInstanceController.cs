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
    public class ProductInstanceController : BaseController
    {
        private readonly IBaseRepository<ProductInstance> _repository;

        public ProductInstanceController(IBaseRepository<ProductInstance> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.ProductInstanceList)]
        public virtual ApiResult<IQueryable<ProductInstanceDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<ProductInstance>();
            var data = _repository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<ProductInstanceDto>();
            return Ok(data);
        }

        [HttpGet("{productInstanceId}")]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.ProductInstanceItem)]
        public virtual ApiResult<ProductInstanceDto> Get(int productInstanceId, int userId)
        {
            var productInstanceDto = _repository.GetAll(x => x.Id == productInstanceId).ProjectTo<ProductInstanceDto>().SingleOrDefault();
            return Ok(productInstanceDto);
        }

        [HttpPost]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.ProductInstanceInsert)]
        public virtual async Task<ApiResult<ProductInstanceDto>> Create(ProductInstanceDto dto, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(dto.ToEntity(), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<ProductInstanceDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }

        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.ProductInstanceDelete, Order = 3)]
        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstAsync(x => x.Id.Equals(id), cancellationToken);
            await _repository.DeleteAsync(model, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [AxAuthorize(StateType = StateType.Authorized, Order = 2, AxOp = AxOp.ProductInstanceUpdate)]
        public virtual async Task<ApiResult<ProductInstanceDto>> Update(ProductInstanceDto dto, CancellationToken cancellationToken)
        {
            var productInstance = await _repository.GetFirstAsync(x => x.Id == dto.Id, cancellationToken);
            if (productInstance == null)
                throw new NotFoundException("نمونه محصولی یافت نشد");

            await _repository.UpdateAsync(dto.ToEntity(productInstance), cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<ProductInstanceDto>().SingleOrDefaultAsync(p => p.Id.Equals(dto.Id), cancellationToken);
            return resultDto;
        }
    }
}
