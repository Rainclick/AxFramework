namespace Entities.Framework
{
    public class HardwareDataHistory : BaseEntity
    {
        public float Cpu { get; set; }
        public float Ram { get; set; }
        public float NetworkIn { get; set; }
        public float NetworkOut { get; set; }
    }
}
