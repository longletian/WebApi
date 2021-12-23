

using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 菜单权限表
    /// </summary>
    [Table(Name = "sys_menu")]
    public class MenuEntity : Entity<long>
    {
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 父菜单id
        /// </summary>
        //public string ParentMenuId { get; set; }

        /// <summary>
        /// 父菜单编码
        /// </summary>
        public string ParentMenuCode { get; set; }
        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; }
        /// <summary>
        /// 菜单跳转url
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// 菜单备注
        /// </summary>
        public string MenuRemark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortNum { get; set; }

        /// <summary>
        /// 一个角色对应多个用户
        /// </summary>

        //[Navigate(ManyToMany = typeof(IdentityMenuPermission))]
        //public virtual ICollection<IdentityPermission> IdentityPermissions { get; set; }

        ////[Navigate("MenuId")]
        //public virtual ICollection<IdentityMenuPermission> IdentityMenuPermissions { get; set; }

    }
}
