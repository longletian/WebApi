using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models.Models.Identity
{
    /// <summary>
    /// 角色信息表
    /// </summary>
   [Table("case_role")]
    public class IdentityRole: Entity
    {
        //public IdentityRole()
        //{
        //    IdentityUserRoleList = new List<IdentityUserRole>();
        //    IdentityUserGroupRoleList = new List<IdentityUserGroupRole>();
        //    IdentityRolePermissionList = new List<IdentityRolePermission>();
        //    MenuModelList = new List<MenuModel>();
        //}

        [Required]
        /// <summary>
        /// 角色code
        /// </summary>
        public string RoleCode { get; set; }

        [Required]
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色父code
        /// </summary>
        public string ParentCode { get; set; }
        /// <summary>
        /// 角色备注
        /// </summary>
        public string RoleRemark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public long SortNum { get; set; }

        //public ICollection<MenuModel> MenuModelList { get; set; }

        //public ICollection<IdentityUserRole>  IdentityUserRoleList { get; set; }

        //public ICollection<IdentityUserGroupRole> IdentityUserGroupRoleList { get; set; }

        //public ICollection<IdentityRolePermission> IdentityRolePermissionList { get; set; }


    }
}
