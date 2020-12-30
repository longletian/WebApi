
using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 用户组角色关联表
    /// </summary>
   [Table(Name= "case_group_role")]
    public class IdentityGroupRole:Entity<long>
    {

        public IdentityGroupRole()
        { 
        
        }

        public IdentityGroupRole(long roleId, long groupId)
        {
            this.RoleId = roleId;
            this.GroupId = groupId;
        }
        public long RoleId { get; set; }

        public long GroupId { get; set; }

        //[Navigate("GroupId")]
        //public IdentityGroup IdentityGroup { get; set; }

        //[Navigate("RoleId")]
        //public IdentityRole IdentityRole { get; set; }
    }
}
