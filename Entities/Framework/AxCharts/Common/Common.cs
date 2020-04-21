namespace Entities.Framework.AxCharts.Common
{

    public class Legend : BaseEntity
    {
        public string Name { get; set; }
        public int AxChartId { get; set; }
        public int? ParentId { get; set; }
    }

    public class AxSeries : BaseEntity
    {
        public string Name { get; set; }
        public AxChartType Type { get; set; }
        public int AxChartId { get; set; }
        public int OrderIndex { get; set; }
    }




    public enum AxChartType
    {
        List = 0,
        Pie = 1,
        Line = 2,
        Bar = 3,
        Area = 4,
        NumericWidget = 5
    }

}
