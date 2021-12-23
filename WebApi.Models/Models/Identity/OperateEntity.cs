using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 功能权限表(按钮操作方面)
    /// </summary>
    [Table(Name = "sys_operate")]
    public class OperateEntity:Entity<long>
    {
        /// <summary>
        /// 功能编码
        /// </summary>
        public string OperateCode { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string OperateName { get; set; }
        /// <summary>
        /// 功能父id 
        /// </summary>
        //public string OperateParentId { get; set; }
        /// <summary>
        /// 功能父code
        /// </summary>
        public string OperateParentCode { get; set; }
        /// <summary>
        /// 功能备注
        /// </summary>
        public string OperateRemark { get; set; }
        /// <summary>
        /// 是否允许
        /// </summary>
        public int IsAble { get; set; }

        //[Navigate(ManyToMany = typeof(IdentityOperatePermission))]
        //public virtual ICollection<IdentityPermission> IdentityPermissions { get; set; }

        ////[Navigate("OperateId")]
        //public virtual ICollection<IdentityOperatePermission> IdentityMenuPermissions { get; set; }

    }
}
