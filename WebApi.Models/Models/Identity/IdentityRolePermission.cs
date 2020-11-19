using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models
{
    /// <summary>
    /// 角色权限关联表
    /// </summary>
  [Table("case_role_permission")]
    public class IdentityRolePermission
    {
        public long Id { get; set; }

        /// <summary>
        /// 权限id
        /// </summary>
        public long PermissionId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>

        public long  RoleId { get; set; }


        //public IdentityPermission  IdentityPermissions { get; set; }

        //public IdentityRole IdentityRoles { get; set; }

    }
}
