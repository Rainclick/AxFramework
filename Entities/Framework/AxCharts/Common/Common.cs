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
        List,
        Pie,
        Line,
        Bar,
        Area,
        NumericWidget
    }

}
