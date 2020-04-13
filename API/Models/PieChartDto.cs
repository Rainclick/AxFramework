using System.Collections.Generic;
using AutoMapper;
using Entities.Framework.AxCharts;
using WebFramework.Api;

namespace API.Models
{
    public class PieChartDto : BaseDto<PieChartDto, PieChart, int>
    {
        public virtual ICollection<LegendDto> Labels { get; set; }
        public virtual AxSeriesDto Series { get; set; }
        public string Title { get; set; }


        public override void CustomMappings(IMappingExpression<PieChart, PieChartDto> mapping)
        {
            mapping.ForMember(
                dest => dest.Title,
                config => config.MapFrom(src => src.AxChart.Title)
            );
        }
    }


}
