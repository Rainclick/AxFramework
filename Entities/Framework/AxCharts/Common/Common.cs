using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework.AxCharts.Common
{

    public class Legend : BaseEntity
    {
        public string Name { get; set; }
        public int AxChartId { get; set; }
    }

    public class AxSeries : BaseEntity
    {
        public string Name { get; set; }
        public AxChartType Type { get; set; }
        [NotMapped]
        public ICollection<object> Data { get; set; }
        public int AxChartId { get; set; }
    }




    public enum AxChartType
    {
        List,
        Pie,
        Line,
        Bar,
        Area,
        Widget
    }

}
