
using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 角色权限关联表
    /// </summary>
    [Table(Name = "case_role_permission")]
    public class IdentityRolePermission : Entity<long>
    {
        public IdentityRolePermission(long permissionId, long roleId)
        {
            this.Role = roleId;
            this.PermissionId = permissionId;
        }


        public long PermissionId { get; set; }

        //[Navigate("PermissionId")]
        //public IdentityPermission IdentityPermission { get; set; }

        public long Role { get; set; }

        //[Navigate("RoleId")]
        //public IdentityRole IdentityRole { get; set; }
    }
}
