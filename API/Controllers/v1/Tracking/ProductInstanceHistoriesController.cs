using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.Tracking;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Utilities;
using Data.Repositories;
using Entities.Framework.Reports;
using Entities.Tracking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Tracking
{
    [ApiVersion("1")]
    public class ProductInstanceHistoriesController : BaseController
    {
        private readonly IBaseRepository<ProductInstanceHistory> _repository;

        public ProductInstanceHistoriesController(IBaseRepository<ProductInstanceHistory> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, AxOp = AxOp.ProductInstanceHistoryList)]
        public virtual ApiResult<IQueryable<ProductInstanceHistoryDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<ProductInstanceHistory>();
            var data = _repository.GetAll(predicate).OrderBy(request.Sort, request.SortType).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ProjectTo<ProductInstanceHistoryDto>();
            return Ok(data);
        }


    }
}
