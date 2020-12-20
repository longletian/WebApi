using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    [Table(Name = "case_role")]
    public class IdentityRole:FullEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色备注
        /// </summary>
        public string RoleRemark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public long SortNum { get; set; }

        ///// <summary>
        ///// 一个角色对应多个用户
        ///// </summary>

        //[Navigate(ManyToMany = typeof(IdentityUserRole))]
        //public virtual ICollection<IdentityUser> IdentityUsers { get; set; }

        ////[Navigate("RoleId")]
        //public virtual ICollection<IdentityUserRole> IdentityGroupUsers { get; set; }

        ///// <summary>
        ///// 一个角色对应多个用户组
        ///// </summary>
        //[Navigate(ManyToMany = typeof(IdentityGroupRole))]
        //public virtual ICollection<IdentityGroup> IdentityGroups { get; set; }

        ////[Navigate("RoleId")]
        //public virtual ICollection<IdentityGroupRole>  IdentityGroupRoles { get; set; }


        ///// <summary>
        ///// 一个角色对应权限
        ///// </summary>
        //[Navigate(ManyToMany = typeof(IdentityRolePermission))]
        //public virtual ICollection<IdentityPermission>  IdentityPermissions { get; set; }

        ////[Navigate("RoleId")]
        //public virtual ICollection<IdentityRolePermission>  IdentityRolePermissions { get; set; }

    }
}
