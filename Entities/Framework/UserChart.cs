using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework.AxCharts.Common;

namespace Entities.Framework
{
    public class UserChart : BaseEntity
    {
        public int UserId { get; set; }
        public int ChartId { get; set; }
        public AxChartType Type { get; set; }
        public int OrderIndex { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
