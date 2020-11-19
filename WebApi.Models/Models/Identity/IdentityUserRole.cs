using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models.Models.Identity
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    public class IdentityUserRole: Entity
    {

        public IdentityUser IdentityUsers { get; set; }

        public IdentityRole IdentityRoles { get; set; }
    }
}
