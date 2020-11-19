using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models.Models.Identity
{
    /// <summary>
    /// 用户组信息表
    /// </summary>
    [Table("case_user_group")]
    public class IdentityUserGroup :Entity
    {
        //public IdentityUserGroup()
        //{
        //    IdentityUserGroupRoleList = new List<IdentityUserGroupRole>();
        //    IdentityGroupUserList = new List<IdentityGroupUser>();
        //}
        [Required]
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserGroupName { get; set; }
        [Required]
        /// <summary>
        /// 用户组code
        /// </summary>
        public string UserGroupCode { get; set; }
        /// <summary>
        /// 用户组父code
        /// </summary>
        public string ParentUserGroupCode { get; set; }
        /// <summary>
        /// 用户组备注
        /// </summary>
        public string  UserGroupRemark { get; set; }


        //public ICollection<IdentityUserGroupRole> IdentityUserGroupRoleList { get; set; }

        //public ICollection<IdentityGroupUser>  IdentityGroupUserList { get; set; }

    }
}
