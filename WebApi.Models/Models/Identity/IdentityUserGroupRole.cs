using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models.Models.Identity
{
    /// <summary>
    /// 用户组角色关联表
    /// </summary>
    public class IdentityUserGroupRole: Entity
    {
        public IdentityUserGroup  IdentityUserGroups { get; set; }

        public IdentityRole IdentityRoles { get; set; }
    }
}
