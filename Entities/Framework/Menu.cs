using System.Collections.Generic;


namespace Entities.Framework
{
    public class Menu : BaseEntity
    {
        public string Title { get; set; }
        public bool Active { get; set; }
        public int Order { get; set; }
        public int? ParentId { get; set; }
        public Menu Parent { get; set; }
        public string Key { get; set; }
        public string Icon { get; set; }
        public virtual ICollection<Menu> Children { get; set; }

    }
}
