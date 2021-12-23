using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 功能权限关联表
    /// </summary>
    [Table(Name = "sys_operate_permission")]
    public class OperatePermissionEntity : Entity<long>
    {
        public OperatePermissionEntity()
        {

        }

        public OperatePermissionEntity(long operateId, long permissionId)
        {
            this.OperateId = operateId;
            this.PermissionId = permissionId;
        }

        public long OperateId { get; set; }

        //[Navigate("OperateId")]
        //public  OperateModel  OperateModel { get; set; }

        public long PermissionId { get; set; }

        //[Navigate("PermissionId")]
        //public  IdentityPermission IdentityPermission { get; set; }

    }
}
