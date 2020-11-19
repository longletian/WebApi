using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models.Models.Identity
{

    [Table("case_group_user")]
   public class IdentityGroupUser
    {
        //public IdentityUser IdentityUsers { get; set; }

        //public IdentityUserGroup IdentityUserGroups { get; set; }

        public long Id { get; set; }

        public long   UserId{ get; set; }

        public long  UserGroupId { get; set; }
    }
}
