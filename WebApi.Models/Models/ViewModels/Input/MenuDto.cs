
using System.Collections.Generic;

namespace WebApi.Models
{
    public class MenuViewDto
    {
        public string MenuName { get; set; }
        public string MenuPath { get; set; }
        public string MenuCode { get; set; }
        public string ParentMenuCode { get; set; }
        public string MenuUrl { get; set; }
        public string MenuRemark { get; set; }
        public List<MenuViewDto> menuList { get; set; }
    }
}
