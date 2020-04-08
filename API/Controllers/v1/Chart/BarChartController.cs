using Common;
using Data.Repositories;
using Entities.Framework.AxCharts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Chart
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarChartController : BaseController
    {
        private readonly IBaseRepository<BarChart> _repository;

        public BarChartController(IBaseRepository<BarChart> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AxAuthorize(StateType = StateType.Ignore, Order = 0, AxOp = AxOp.UserList, ShowInMenu = true)]
        public ApiResult<BarChart> Get(int chartId)
        {
            var chart = _repository.GetFirst(x => x.Id == chartId);

            return Ok(chart);
        }

    }
}