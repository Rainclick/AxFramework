using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace Entities.Framework.Reports
{
    public class NewReport : BaseEntity
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public int TakeSize { get; set; }
        public ICollection<ColumnReport> Columns { get; set; }
    }

    public class NewReportValidator : AbstractValidator<NewReport> { }

    public class ColumnReport : BaseEntity
    {
        public ColumnType ColumnType { get; set; }
        public string Name { get; set; }
        public int ReportId { get; set; }
        [ForeignKey("ReportId")]
        public NewReport Report { get; set; }
        public string TargetType { get; set; }
        public string JoinId { get; set; }
        public OrderByReport OrderByReport { get; set; }
    }
    public class ColumnReportValidator : AbstractValidator<ColumnReport> { }

    public class OrderByReport : BaseEntity
    {
        public int OrderIndex { get; set; }
        public OrderByType OrderByType { get; set; }
        public int? ColumnReportId { get; set; }
        [ForeignKey("ColumnReportId")]
        public ColumnReport ColumnReport { get; set; }
    }

    public class Filter : BaseEntity
    {
        public string Property { get; set; }
        public OperationType Operation { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public ConnectorType Connector { get; set; }
        public int? ReportId { get; set; }
        [ForeignKey("ReportId")]
        public NewReport Report { get; set; }
        public bool IsCalculation { get; set; }
        public bool IsActive { get; set; } = true;

        //public string Type { get; set; }
    }
    public class FilterValidator : AbstractValidator<Filter> { }


    public enum OrderByType
    {
        Asc,
        Desc
    }

    public enum ConnectorType
    {
        And,
        Or
    }
    public enum ColumnType
    {
        String,
        Datetime,
        Number,
        Boolean,
        Object
    }

}
