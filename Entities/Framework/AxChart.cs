using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework
{
    public class AxChart : BaseEntity
    {
        public string Title { get; set; }
        public bool Active { get; set; }
        public AxChartType Type { get; set; }
        public long? SystemId { get; set; }
        [ForeignKey("SystemId")]
        public Menu System { get; set; }
    }

    public class PieChart : AxChart
    {

    }

    public enum AxChartType
    {
        List,
        Pie,
        Line,
        Column,
        Area
    }


}
