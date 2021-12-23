using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 数据权限关联表
    /// </summary>
    [Table(Name="sys_file_permission")]
    public class DataPermissionEntity:Entity<long>
    {
        public DataPermissionEntity()
        { 
        
        }

        public DataPermissionEntity(long fileDataId, long permissionId)
        {
            this.FileDataId = fileDataId;
            this.PermissionId = permissionId;
        }

        public long FileDataId { get; set; }

        //[Navigate("FileDataId")]
        //public  FileDataModel  FileDataModel  { get; set; }

        public long PermissionId { get; set; }

        //[Navigate("PermissionId")]
        //public  IdentityPermission IdentityPermission { get; set; }

    }
}
