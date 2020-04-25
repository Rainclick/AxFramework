using System.Collections.Generic;
using Entities.Framework.AxCharts.Common;
using WebFramework.Api;

namespace API.Models
{

    public class AxSeriesDto : BaseDto<AxSeriesDto, AxSeries, int>
    {
        public string Name { get; set; }
        public ICollection<object> Data { get; set; }
    }
}
