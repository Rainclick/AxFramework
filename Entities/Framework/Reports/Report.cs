using System.Collections.Generic;
using Common;

namespace Entities.Framework.Reports
{
    public class Report : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<AxFilter> Filters { get; set; }
        public string TypeName { get; set; }
        public string ResultTypeName { get; set; }
        public int TakeSize { get; set; }
        public string Sort { get; set; }
        public SortType SortType { get; set; }
        public ReportType ExecuteType { get; set; }
        public string GroupBy { get; set; }
    }

    public class C
    {
        public string Title { get; set; }
        public ColumnType ColumnType { get; set; }
    }

    public enum ColumnType
    {
        String,
        Datetime,
        Number,
        Boolean
    }

    public enum ReportType
    {
        All,
        First,
        Count
    }
}
