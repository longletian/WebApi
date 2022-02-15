using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models
{

    [Table(Name = "case_role_group")]
    /// <summary>
    /// 角色组
    /// </summary>
    public class IdentityRoleGroup:FullEntity
    {
        /// <summary>
        /// 角色code
        /// </summary>
        public string RoleGroupCode { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleGroupName { get; set; }

        /// <summary>
        /// 角色父组id
        /// </summary>
        //public string ParentRoleGroupId { get; set; }

        /// <summary>
        /// 角色父组Code
        /// </summary>
        public string ParentRoleGroupCode { get; set; }

        /// <summary>
        /// 角色备注
        /// </summary>
        public string RoleGroupRemark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public long SortNum { get; set; }
    }

}
