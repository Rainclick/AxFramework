using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Entities.Framework.AxCharts;
using Entities.Framework.AxCharts.Common;
using WebFramework.Api;

namespace API.Models
{
    public class BarChartDto : BaseDto<BarChartDto, BarChart, int>
    {
        public string XField { get; set; }
        public string YField { get; set; }
        public virtual List<AxSeriesDto> Series { get; set; }
        public int? NextChartId { get; set; }
        public AxChartType NextChartType { get; set; }
        public string Title { get; set; }

        public override void CustomMappings(IMappingExpression<BarChart, BarChartDto> mapping)
        {
            mapping.ForMember(
                dest => dest.Title,
                config => config.MapFrom(src => src.AxChart.Title)
            );
            mapping.ForMember(
                dest => dest.NextChartId,
                config => config.MapFrom(src => src.AxChart.NextChartId)
            );
            mapping.ForMember(
                dest => dest.NextChartType,
                config => config.MapFrom(src => src.AxChart.ChartType)
            );

            mapping.ForMember(
                dest => dest.Series,
                config => config.MapFrom(src => src.Series.OrderBy(x => x.OrderIndex))
            );

        }


    }


}
