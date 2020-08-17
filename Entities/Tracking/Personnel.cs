using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework;

namespace Entities.Tracking
{
    public class Personnel : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string Code { get; set; }
        public int FactoryId { get; set; }
        [ForeignKey("FactoryId")]
        public Factory Factory { get; set; }

    }
}
