using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models.Models.Identity
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class IdentityPermission : Entity
    {
        public IdentityPermission()
        {
            IdentityRolePermissionList = new List<IdentityRolePermission>();
        }

        [Required]
        /// <summary>
        /// 权限code
        /// </summary>
        public string PermissionCode { get; set; }
        [Required]
        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName { get; set; }
        /// <summary>
        /// 权限父用户组id
        /// </summary>
        public string ParentPermissionCode { get; set; }
        /// <summary>
        /// 权限备注
        /// </summary>
        public string PermissionRemark { get; set; }

        /// <summary>
        /// 权限排序
        /// </summary>
        public long SortNum { get; set; }


        public List<IdentityRolePermission> IdentityRolePermissionList { get; set; }
    }
}
