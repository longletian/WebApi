using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models.Models.Identity
{
   public class IdentityGroupUser:Entity
    {
        public IdentityUser IdentityUsers { get; set; }

        public IdentityUserGroup IdentityUserGroups { get; set; }
    }
}
