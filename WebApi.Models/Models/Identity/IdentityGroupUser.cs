
using FreeSql.DataAnnotations;

namespace WebApi.Models
{

    /// <summary>
    /// 用户组和用户关系表
    /// </summary>
    [Table(Name = "case_group_user")]
    public class IdentityGroupUser : Entity<long>
    {
        public IdentityGroupUser(long userId,long groupId)
        {
            this.UserId = userId;
            this.GroupId = groupId;
        }
        public long UserId { get; set; }

        public long GroupId { get; set; }

        //[Navigate("GroupId")]
        //public IdentityGroup IdentityGroup {get;set;}

        //[Navigate("UserId")]
        //public IdentityUser IdentityUser { get; set; }
    }
}
    