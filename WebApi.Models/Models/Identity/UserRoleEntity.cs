
using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table(Name = "sys_user_role")]
    public class UserRoleEntity : Entity<long>
    {
        /// <summary>
        /// 当类没有创建构造函数的时候，会自动生成一个默认的空构造函数
        /// </summary>
        public UserRoleEntity()
        { 
        
        }

        public UserRoleEntity(long userId, long roleId)
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
