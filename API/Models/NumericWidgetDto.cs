using Entities.Framework.AxCharts;
using WebFramework.Api;

namespace API.Models
{
    public class NumericWidgetDto: BaseDto<NumericWidgetDto, NumericWidget, int>
    {
        public string Label { get; set; }
        public string Icon { get; set; }
        public string LastUpdated { get; set; }
        public long Data { get; set; }
    }
}
