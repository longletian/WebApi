

using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    /// <summary>
    /// 用户组信息表
    /// </summary>
    [Table(Name="sys_user_group")]
    public class GroupEntity :FullEntity
    {
        /// <summary>
        /// 组名称
        /// </summary>
        [Column(StringLength = 60)]
        public string GroupName { get; set; }
        /// <summary>
        /// 用户组code
        /// </summary>
        [Column(StringLength =20)]
        public string GroupCode { get; set; }

        //public string ParentGroupId { get; set; }
        /// <summary>
        /// 用户组父code
        /// </summary>
        [Column(StringLength = 20)]
        public string ParentGroupCode { get; set; }

        /// <summary>
        /// 用户组排序
        /// </summary>
        public long Sort { get; set; }
        /// <summary>
        /// 用户组备注
        /// </summary>
        public string  Remark { get; set; }

        //[Navigate(ManyToMany = typeof(IdentityGroupUser))]
        //public virtual ICollection<IdentityUser>  IdentityUsers { get; set; }

        //[Navigate("GroupId")]
        //public virtual ICollection<IdentityGroupUser> IdentityGroupUsers { get; set; }


        //[Navigate(ManyToMany = typeof(IdentityGroupRole))]
        //public virtual ICollection<IdentityRole> IdentityRole { get; set; }

        //[Navigate("GroupId")]
        //public virtual ICollection<IdentityGroupRole> IdentityGroupRoles    { get; set; }

    }
}
