using Entities.Framework;

namespace Entities.Tracking
{
    public class Shift : BaseEntity
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
