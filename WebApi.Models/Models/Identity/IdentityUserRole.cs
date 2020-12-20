
using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table(Name = "case_user_role")]
    public class IdentityUserRole : Entity<long>
    {
        public IdentityUserRole(long userId, long roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }

        public long UserId { get; set; }

        public long RoleId { get; set; }


        //[Navigate("RoleId")]
        //public IdentityRole IdentityRole { get; set; }

        //[Navigate("UserId")]
        //public IdentityUser IdentityUser { get; set; }
    }
}
