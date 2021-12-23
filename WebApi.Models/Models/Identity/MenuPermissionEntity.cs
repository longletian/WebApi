using FreeSql.DataAnnotations;

namespace WebApi.Models
{

    /// <summary>
    /// 菜单权限关联表(菜单操作方面)
    /// </summary>
    [Table(Name = "sys_menu_permission")]
    public class MenuPermissionEntity : Entity<long>
    {
        public MenuPermissionEntity()
        { 
        
        }

        public MenuPermissionEntity(long menuId, long permissionId)
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
