using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework.AxCharts.Common;

namespace Entities.Framework.AxCharts
{
    public class LineChart : BaseEntity
    {
        public virtual ICollection<Legend> Legends { get; set; }

        public int AxChartId { get; set; }
        [ForeignKey("AxChartId")]
        public AxChart AxChart { get; set; }
    }
}
