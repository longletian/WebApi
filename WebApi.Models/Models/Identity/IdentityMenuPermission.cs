using FreeSql.DataAnnotations;

namespace WebApi.Models
{

    /// <summary>
    /// 菜单权限关联表
    /// </summary>
    [Table(Name = "case_menu_permission")]
    public class IdentityMenuPermission : Entity<long>
    {
        public IdentityMenuPermission()
        { 
        
        }

        public IdentityMenuPermission(long menuId, long permissionId)
        {
            this.MenuId = menuId;
            this.PermissionId = permissionId;
        }

        public long MenuId { get; set; }

        //[Navigate("MenuId")]
        //public MenuModel MenuModel { get; set; }

        public long PermissionId { get; set; }

        //[Navigate("PermissionId")]
        //public  IdentityPermission IdentityPermission { get; set; }
    }
}
