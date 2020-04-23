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
            var chart = _repository.GetAll(x => x.Id == chartId).Include(x => x.Report).ThenInclude(x => x.Filters).FirstOrDefault();
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
                            lineChart.Series.Add(new AxSeriesDto { Name = "خطا", Id = i });
                            lineChart.Series[i].Data = new List<object> { 15, 10, 8 };
                        }
                        if (i == 1)
                        {
                            lineChart.Series.Add(new AxSeriesDto { Name = "هشدار", Id = i });
                            lineChart.Series[i].Data = new List<object> { 12, 8, 5 };
                        }
                        if (i == 2)
                        {
                            lineChart.Series.Add(new AxSeriesDto { Name = "اطلاعات", Id = i });
                            lineChart.Series[i].Data = new List<object> { 10, 9, 0 };
                        }
                    }
                    lineChart.Labels = new List<string> { "شنبه", "یکشنبه", "دوشنبه" };
                    return Ok(lineChart);
                }
            }
            if (chart?.ChartType == AxChartType.Bar)
            {
                var barChart = _barChartRepository.GetAll(x => x.AxChartId == chartId).ProjectTo<BarChartDto>().FirstOrDefault();
                if (barChart != null && barChart.Series?.Count > 0)
                {
                    var date = DateTime.Now.AddDays(-7);
                    var data0 = _loginlogRepository.GetAll(x => x.InsertDateTime.Date >= date.Date).GroupBy(x => x.InsertDateTime.Date).Select(x => new { Count = x.Count() as object, x.Key }).ToList();
                    //var data = chart.Report.Execute();
                    var a = data0.Select(x => x.Count).ToList();
                    barChart.Series[0] = new AxSeriesDto { Data = a, Name = "تعداد ورود به سیستم" };
                    //foreach (var item in data0)
                    //{

                    //}
                    //for (var i = 0; i < barChart.Series.Count; i++)
                    //{

                    //    if (i == 0)
                    //        barChart.Series[i].Data = new List<object> { 15, 10, 8 };
                    //    if (i == 1)
                    //        barChart.Series[i].Data = new List<object> { 12, 8, 5 };
                    //    if (i == 2)
                    //        barChart.Series[i].Data = new List<object> { 16, 9 };
                    //}
                    barChart.Labels = data0.Select(x => x.Key.ToPerDateString("d MMMM")).ToList();
                    return Ok(barChart);
                }
            }
            if (chart?.ChartType == AxChartType.List)
            {
                var assembly = typeof(UserDto).Assembly;
                var resultDtoType = assembly.GetType(chart.Report.ResultTypeName);
                var scope = AutoFacSingleton.Instance.BeginLifetimeScope();
                var data0 = chart.Report.Execute(scope);
                var data = typeof(ReportExtensions).GetMethod("AxProjectTo")?.MakeGenericMethod(resultDtoType).Invoke(null, new[] { data0 });
                if (data is IQueryable<dynamic> data2)
                    data = data2.ToList();
                var columns = resultDtoType.GetCustomAttributesOfType();
                var listChart = new ListChartDto { Id = chartId, Title = chart.Title, NextChartId = chart.NextChartId, NextChartType = chart.NextChartType, Data = data, Columns = columns };
                scope.Dispose();
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