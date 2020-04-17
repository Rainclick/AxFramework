using System.Collections.Generic;
using AutoMapper;
using Entities.Framework;
using WebFramework.Api;

namespace API.Models
{
    public class MenuDto : BaseDto<MenuDto, Menu, int>
    {
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string Key { get; set; }
        public string Icon { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<MenuDto> Children { get; set; }

    }
}
