using System.Collections.Generic;
using Common;


namespace Entities.Framework
{
    public class Menu : BaseEntity
    {
        public string Title { get; set; }
        public bool Active { get; set; }
        public bool ShowInMenu { get; set; }
        public int? ParentId { get; set; }
        public Menu Parent { get; set; }
        public string Key { get; set; }
        public string Icon { get; set; }
        public virtual ICollection<Menu> Children { get; set; }
        public AxOp AxOp { get; set; }
        public int OrderId { get; set; }
        public string Url { get; set; }
    }
}
