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
using Entities.Framework;
using Entities.Framework.AxCharts.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;
using WebFramework.UserData;

namespace API.Controllers.v1.Chart
{
    [ApiVersion("1")]
    public class ChartsController : BaseController
    {
        private readonly IBaseRepository<AxChart> _repository;
        private readonly IBaseRepository<PieChart> _pieRepository;
        private readonly IBaseRepository<BarChart> _barChartRepository;
        private readonly IBaseRepository<NumericWidget> _numericWidgetRepository;
        private readonly IBaseRepository<LoginLog> _loginlogRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<LineChart> _lineRepository;
        private readonly IBaseRepository<Log> _logRepository;
        private IHubContext<ChartHub> _hub;
        public ChartsController(IBaseRepository<AxChart> repository, IBaseRepository<PieChart> pieRepository, IBaseRepository<BarChart> barChartRepository,
            IBaseRepository<NumericWidget> numericWidgetRepository, IBaseRepository<LoginLog> loginlogRepository, IBaseRepository<User> userRepository,
            IBaseRepository<LineChart> lineRepository, IBaseRepository<Log> logRepository, IHubContext<ChartHub> hub)
        {
            _repository = repository;
            _pieRepository = pieRepository;
            _barChartRepository = barChartRepository;
            _numericWidgetRepository = numericWidgetRepository;
            _loginlogRepository = loginlogRepository;
            _userRepository = userRepository;
            _lineRepository = lineRepository;
            _logRepository = logRepository;
            _hub = hub;
        }

        [HttpGet("[action]/{chartId}/{filter?}")]
        [AxAuthorize(StateType = StateType.Ignore)]
        public ApiResult<dynamic> GetChart(int chartId, int? filter = null)
        {
            var chart = _repository.GetAll(x => x.Id == chartId).Include(x => x.Report).ThenInclude(x=> x.Filters).FirstOrDefault();
            if (chart?.ChartType == AxChartType.Pie)
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

            if (chart?.ChartType == AxChartType.NumericWidget)
            {
                var numericWidget = _numericWidgetRepository.GetAll(x => x.AxChartId == chartId).ProjectTo<NumericWidgetDto>().FirstOrDefault();
                if (numericWidget != null)
                {
                    var data = chart.Report.Execute();
                    numericWidget.Data = (int)data;
                    numericWidget.LastUpdated = DateTime.Now.ToPerDateTimeString("yyyy/MM/dd HH:mm:ss");
                    return Ok(numericWidget);
                }
                if (numericWidget != null && numericWidget.Id == 5)
                {
                    numericWidget.Data = _logRepository.Count(x => x.Level == "Error" && x.Logged.Date == DateTime.Now.Date);
                    numericWidget.LastUpdated = DateTime.Now.ToPerDateTimeString("yyyy/MM/dd HH:mm:ss");
                    return Ok(numericWidget);
                }
                if (numericWidget != null && numericWidget.Id == 9)
                {
                    var lastHour = DateTime.Now.AddHours(-1);
                    numericWidget.Data = _userRepository.Count(x => x.LastLoginDate.HasValue && x.LastLoginDate.Value >= lastHour);
                    numericWidget.LastUpdated = DateTime.Now.ToPerDateTimeString("yyyy/MM/dd HH:mm:ss");
                    return Ok(numericWidget);
                }
            }
            if (chart?.ChartType == AxChartType.Line)
            {
                var lineChart = _lineRepository.GetAll(x => x.AxChartId == chartId).ProjectTo<LineChartDto>().FirstOrDefault();
                if (lineChart != null)
                {
                    lineChart.Series = new List<AxSeriesDto>();
                    for (var i = 0; i < 7; i++)
                    {
                        if (i == 0)
                        {
                            lineChart.Series.Add(new AxSeriesDto { Name = "شنبه", Id = i });
                            lineChart.Series[i].Data = new List<object> { 15, 10, 8 };
                        }
                        if (i == 1)
                        {
                            lineChart.Series.Add(new AxSeriesDto { Name = "یکشنبه", Id = i });
                            lineChart.Series[i].Data = new List<object> { 12, 8, 5 };
                        }
                        if (i == 2)
                        {
                            lineChart.Series.Add(new AxSeriesDto { Name = "دوشنبه", Id = i });
                            lineChart.Series[i].Data = new List<object> { 10, 9 };
                        }
                    }
                    lineChart.Labels = new List<string> { "خطا", "هشدار", "اطلاعات" };
                    return Ok(lineChart);
                }
            }
            if (chart?.ChartType == AxChartType.Bar)
            {
                var barChart = _barChartRepository.GetAll(x => x.AxChartId == chartId).ProjectTo<BarChartDto>().FirstOrDefault();
                if (barChart != null && barChart.Series?.Count > 0)
                {
                    for (var i = 0; i < barChart.Series.Count; i++)
                    {
                        if (i == 0)
                            barChart.Series[i].Data = new List<object> { 15, 10, 8 };
                        if (i == 1)
                            barChart.Series[i].Data = new List<object> { 12, 8, 5 };
                        if (i == 2)
                            barChart.Series[i].Data = new List<object> { 16, 9 };
                    }
                    barChart.Labels = new List<string> { "لیبل 1", "لیبل 2", "لیبل 3" };
                    return Ok(barChart);
                }
            }
            if (chart?.ChartType == AxChartType.List)
            {
                var assembly = typeof(UserDto).Assembly;
                var resultDtoType = assembly.GetType(chart.Report.ResultTypeName);

                var data0 = chart.Report.Execute();
                var data = typeof(ReportExtensions).GetMethod("AxProjectTo")?.MakeGenericMethod(resultDtoType).Invoke(null, new object[] { data0 });
                var columns = resultDtoType.GetCustomAttributesOfType();
                var listChart = new ListChartDto { Id = chartId, Title = chart.Title, NextChartId = chart.NextChartId, NextChartType = chart.NextChartType, Data = data, Columns = columns };
                return Ok(listChart);
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