using System.Collections.Generic;
using AutoMapper;
using Entities.Framework.AxCharts;
using Entities.Framework.AxCharts.Common;
using WebFramework.Api;

namespace API.Models
{
    public class PieChartDto : BaseDto<PieChartDto, PieChart, int>
    {
        public virtual ICollection<LegendDto> Labels { get; set; }
        public virtual AxSeriesDto Series { get; set; }
        public string Title { get; set; }
        public int? NextChartId { get; set; }
        public AxChartType NextChartType { get; set; }


        public override void CustomMappings(IMappingExpression<PieChart, PieChartDto> mapping)
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

        }
    }


}
