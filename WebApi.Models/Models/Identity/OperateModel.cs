using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 功能权限表
    /// </summary>
    [Table(Name = "case_operate")]
    public class OperateModel:Entity<long>
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
        public long OperateParentId { get; set; }
        /// <summary>
        /// 功能备注
        /// </summary>
        public string OperateRemark { get; set; }

        //[Navigate(ManyToMany = typeof(IdentityOperatePermission))]
        //public virtual ICollection<IdentityPermission> IdentityPermissions { get; set; }

        ////[Navigate("OperateId")]
        //public virtual ICollection<IdentityOperatePermission> IdentityMenuPermissions { get; set; }

    }
}
