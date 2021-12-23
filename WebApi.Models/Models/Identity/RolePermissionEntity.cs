
using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 角色权限关联表
    /// </summary>
    [Table(Name = "sys_role_permission")]
    public class RolePermissionEntity : Entity<long>
    {
        public RolePermissionEntity()
        { 
        
        }

        public RolePermissionEntity(long permissionId, long roleId)
        {
            this.RoleId = roleId;
            this.PermissionId = permissionId;
        }


        public long PermissionId { get; set; }

        //[Navigate("PermissionId")]
        //public IdentityPermission IdentityPermission { get; set; }

        public long RoleId { get; set; }

        //[Navigate("RoleId")]
        //public IdentityRole IdentityRole { get; set; }
    }
}
