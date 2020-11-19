using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models.Models.Identity
{
    /// <summary>
    /// 用户组角色关联表
    /// </summary>
   [Table("case_usergroup_role")]
    public class IdentityUserGroupRole
    {
        public long Id { get; set; }

        public long  UserGroupId { get; set; }

        public long  RoleId { get; set; }

        //public IdentityUserGroup  IdentityUserGroups { get; set; }

        //public IdentityRole IdentityRoles { get; set; }
    }
}
