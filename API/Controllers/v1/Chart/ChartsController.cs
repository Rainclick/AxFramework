﻿using System.Collections.Generic;
using Common;
using Data.Repositories;
using Entities.Framework.AxCharts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API.Hubs;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.Framework.AxCharts.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Chart
{
    [ApiVersion("1")]
    public class ChartsController : BaseController
    {
        private readonly IBaseRepository<AxChart> _repository;
        private readonly IBaseRepository<PieChart> _pieRepository;
        private IHubContext<ChartHub> _hub;
        public ChartsController(IBaseRepository<AxChart> repository, IBaseRepository<PieChart> pieRepository, IHubContext<ChartHub> hub)
        {
            _repository = repository;
            _pieRepository = pieRepository;
            _hub = hub;
        }

        [HttpGet("[action]/{chartId}/{filter?}")]
        [AxAuthorize(StateType = StateType.Ignore)]
        public ApiResult<dynamic> GetChart(int chartId, string filter = null)
        {
            var chart = _repository.GetFirst(x => x.Id == chartId);
            if (chart.ChartType == AxChartType.Pie)
            {
                var pieChart = _pieRepository.GetAll(x => x.AxChartId == chartId).Include(x => x.Series).Include(x => x.Labels).ProjectTo<PieChartDto>().FirstOrDefault();
                if (pieChart != null)
                    pieChart.Series.Data = GetChartData(chart.ReportId, filter);//where filter
                return Ok(pieChart);
            }
            return Ok(chart);
        }

        private List<object> GetChartData(int? reportId, string filter)
        {
            if (reportId == 1)
            {
                return new List<object> { 25, 65, 10 };
            }
            if (reportId == 2)
            {
                return new List<object> { 15, 10 };
            }
            return null;
        }

    }
}