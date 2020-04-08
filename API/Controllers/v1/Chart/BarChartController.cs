using Common;
using Data.Repositories;
using Entities.Framework.AxCharts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Chart
{
    [ApiVersion("1")]
    public class BarChartController : BaseController
    {
        private readonly IBaseRepository<BarChart> _repository;
        private IHubContext<ChartHub> _hub;
        public BarChartController(IBaseRepository<BarChart> repository,IHubContext<ChartHub> hub)
        {
            _repository = repository;
            _hub = hub;
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