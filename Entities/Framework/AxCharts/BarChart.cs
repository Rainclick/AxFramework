using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework.AxCharts.Common;

namespace Entities.Framework.AxCharts
{
    public class BarChart : BaseEntity
    {
        public string XField { get; set; }
        [NotMapped]
        public ICollection<object> XData { get; set; }
        public string YField { get; set; }
        [NotMapped]
        public ICollection<object> YData { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public int? SystemId { get; set; }
        [ForeignKey("SystemId")]
        public Menu System { get; set; }
        public virtual ICollection<AxSeries> Series { get; set; }
        public virtual ICollection<Legend> Legends { get; set; }
    }
}
