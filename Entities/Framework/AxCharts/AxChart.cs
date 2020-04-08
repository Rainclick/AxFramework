using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework.AxCharts
{
    public abstract class AxChart : BaseEntity
    {
        public string Title { get; set; }
        public bool Active { get; set; }
        public int? SystemId { get; set; }
        [ForeignKey("SystemId")]
        public Menu System { get; set; }
        public int? ReportId { get; set; }
    }
}
