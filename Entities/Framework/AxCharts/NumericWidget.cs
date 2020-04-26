using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework.AxCharts
{

    public class NumericWidget : BaseEntity
    {
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int AxChartId { get; set; }
        [ForeignKey("AxChartId")]
        public AxChart AxChart { get; set; }
    } 
}
