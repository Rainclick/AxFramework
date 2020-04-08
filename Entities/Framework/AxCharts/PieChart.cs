using System.Collections.Generic;
using Entities.Framework.AxCharts.Common;

namespace Entities.Framework.AxCharts
{
    public class PieChart : AxChart
    {
        public virtual ICollection<AxSeries> Series { get; set; }
        public virtual  ICollection<Legend> Legends { get; set; }
    }
}
