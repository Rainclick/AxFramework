using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework.AxCharts.Common;
using FluentValidation;

namespace Entities.Framework.AxCharts
{
    public class BarChart : BaseEntity
    {
        public string XField { get; set; }
        public string YField { get; set; }
        public virtual ICollection<AxSeries> Series { get; set; }
        public virtual ICollection<Legend> Legends { get; set; }

        public int AxChartId { get; set; }
        [ForeignKey("AxChartId")]
        public AxChart AxChart { get; set; }
    }
    public class BarChartValidator : AbstractValidator<BarChart> { }
}
