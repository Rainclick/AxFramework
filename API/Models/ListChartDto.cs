using System.Collections.Generic;
using System.Linq;
using Entities.Framework.AxCharts;
using Entities.Framework.AxCharts.Common;
using WebFramework.Api;

namespace API.Models
{
    public class ListChartDto : BaseDto<ListChartDto, AxChart, int>
    {
        public string Title { get; set; }
        public int? NextChartId { get; set; }
        public AxChartType? NextChartType { get; set; }
        public IQueryable<object> Data { get; set; }
        public Dictionary<string, string> Columns { get; set; }

    }
}
