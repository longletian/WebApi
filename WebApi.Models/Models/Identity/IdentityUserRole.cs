using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
  [Table("case_user_role")]
    public class IdentityUserRole
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public long  RoleId { get; set; }

        //public IdentityUser IdentityUsers { get; set; }

        //public IdentityRole IdentityRoles { get; set; }
    }
}
