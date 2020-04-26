using Entities.Framework.AxCharts.Common;
using WebFramework.Api;

namespace API.Models
{
    public class LegendDto : BaseDto<LegendDto, Legend, int>
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Tag { get; set; }
    }
}
