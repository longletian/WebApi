using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models
{
    [Table(Name = "case_role_rolegroup")]
    /// <summary>
    /// 角色组角色关联表
    /// </summary>
    public class IdentityRoleGroupRole : Entity<long>
    {

        public IdentityRoleGroupRole(long roleGroupId, long roleId)
        {
            this.RoleGroupId = roleGroupId;
            this.RoleId = roleId;
        }

        public long RoleGroupId { get; set; }

        public long RoleId { get; set; }
    }
}
