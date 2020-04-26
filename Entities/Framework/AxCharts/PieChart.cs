using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework.AxCharts.Common;

namespace Entities.Framework.AxCharts
{
    public class PieChart : BaseEntity
    {
        [ForeignKey("SeriesId")]
        public virtual AxSeries Series { get; set; }
        public int SeriesId { get; set; }
        public int AxChartId { get; set; }
        [ForeignKey("AxChartId")]
        public AxChart AxChart { get; set; }
    }
}
