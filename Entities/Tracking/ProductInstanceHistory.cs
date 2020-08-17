using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework;

namespace Entities.Tracking
{
    public class ProductInstanceHistory : BaseEntity
    {
        public int ProductInstanceId { get; set; }
        [ForeignKey("ProductInstanceId")]
        public ProductInstance ProductInstance { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int PersonnelId { get; set; }
        [ForeignKey("PersonnelId")]
        public Personnel Personnel { get; set; }
        public int OpId { get; set; }
        [ForeignKey("OpId")]
        public OperationStation OperationStation { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }
    }
}
