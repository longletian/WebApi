using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    [Table(Name = "case_permission")]
    public class IdentityPermission:Entity<long>
    { 
        /// <summary>
        /// 权限code
        /// </summary>
        public string PermissionCode { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName { get; set; }
        /// <summary>
        /// 权限父用户组id
        /// </summary>
        //public string ParentPermissionId { get; set; }
        /// <summary>
        /// 权限父用户组Code
        /// </summary>
        public string ParentPermissionCode { get; set; }
        /// <summary>
        /// 权限备注
        /// </summary>
        public string PermissionRemark { get; set; }
        /// <summary>
        /// 权限排序
        /// </summary>
        public int SortNum { get; set; }

        ///// <summary>
        ///// 一个角色对应权限
        ///// </summary>
        //[Navigate(ManyToMany = typeof(IdentityRolePermission))]
        //public virtual ICollection<IdentityRole> IdentityPermissions { get; set; }

        ////[Navigate("PermissionId")]
        //public virtual ICollection<IdentityRolePermission> IdentityRolePermissions { get; set; }


        //[Navigate(ManyToMany = typeof(IdentityMenuPermission))]
        //public virtual ICollection<MenuModel>  MenuModels { get; set; }

        ////[Navigate("PermissionId")]
        //public virtual ICollection<IdentityMenuPermission> IdentityMenuPermissions { get; set; }


        //[Navigate(ManyToMany = typeof(IdentityDataPermission))]
        //public virtual ICollection<FileDataModel>  FileDataModels { get; set; }

        ////[Navigate("PermissionId")]
        //public virtual ICollection<IdentityDataPermission>   IdentityDataPermissions { get; set; }


        //[Navigate(ManyToMany = typeof(IdentityOperatePermission))]
        //public virtual ICollection<OperateModel>  OperateModels { get; set; }

        ////[Navigate("PermissionId")]
        //public virtual ICollection<IdentityOperatePermission>  IdentityOperatePermissions { get; set; }

    }
}
