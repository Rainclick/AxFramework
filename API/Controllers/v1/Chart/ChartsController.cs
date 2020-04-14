using System;
using System.Collections.Generic;
using Common;
using Data.Repositories;
using Entities.Framework.AxCharts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API.Hubs;
using API.Models;
using AutoMapper.QueryableExtensions;
using Common.Utilities;
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
        private readonly IBaseRepository<NumericWidget> _numericWidgetRepository;
        private IHubContext<ChartHub> _hub;
        public ChartsController(IBaseRepository<AxChart> repository, IBaseRepository<PieChart> pieRepository, IBaseRepository<NumericWidget> numericWidgetRepository, IHubContext<ChartHub> hub)
        {
            _repository = repository;
            _pieRepository = pieRepository;
            _numericWidgetRepository = numericWidgetRepository;
            _hub = hub;
        }

        [HttpGet("[action]/{chartId}/{filter?}")]
        [AxAuthorize(StateType = StateType.Ignore)]
        public ApiResult<dynamic> GetChart(int chartId, int? filter = null)
        {
            var chart = _repository.GetFirst(x => x.Id == chartId);
            if (chart.ChartType == AxChartType.Pie)
            {
                var pieChart = _pieRepository.GetAll(x => x.AxChartId == chartId).Include(x => x.Series).Include(x => x.Labels).ProjectTo<PieChartDto>().FirstOrDefault();
                if (pieChart != null)
                {
                    pieChart.Series.Data = GetChartData(chart.ReportId, filter); //where filter
                    if (filter != null)
                        pieChart.Labels = pieChart.Labels.Where(x => x.ParentId == filter).ToList();
                }

                return Ok(pieChart);
            }

            if (chart.ChartType == AxChartType.NumericWidget)
            {
                var numericWidget = _numericWidgetRepository.GetAll(x => x.AxChartId == chartId).ProjectTo<NumericWidgetDto>().FirstOrDefault();
                if (numericWidget != null)
                {
                    numericWidget.Data = 285;
                    numericWidget.LastUpdated = DateTime.Now.ToPerDateTimeString("yyyy/MM/dd HH:mm");
                    return Ok(numericWidget);
                }
            }
            return Ok(chart);
        }

        private List<object> GetChartData(int? reportId, int? filter)
        {
            if (reportId == 1)
            {
                return new List<object> { 25, 65, 10 };
            }
            if (reportId == 2)
            {
                if (filter == 2)
                    return new List<object> { 15, 10 };
                if (filter == 3)
                    return new List<object> { 65 };
                if (filter == 7)
                    return new List<object> { 7, 3 };
            }
            return null;
        }

    }
}