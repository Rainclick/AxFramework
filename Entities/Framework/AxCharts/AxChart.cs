using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework.AxCharts.Common;

namespace Entities.Framework.AxCharts
{
    public class AxChart : BaseEntity
    {
        public string Title { get; set; }
        public bool Active { get; set; }
        public int? SystemId { get; set; }
        [ForeignKey("SystemId")]
        public Menu System { get; set; }
        public int? ReportId { get; set; }
        public int? NextChartId { get; set; }
        public AxChartType? NextChartType { get; set; }
        public AxChartType ChartType { get; set; }
    }
}
