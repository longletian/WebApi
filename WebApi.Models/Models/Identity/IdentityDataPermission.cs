using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 数据权限关联表
    /// </summary>
    [Table(Name="case_file_permission")]
    public class IdentityDataPermission:Entity<long>
    {
        public IdentityDataPermission()
        { 
        
        }

        public IdentityDataPermission(long fileDataId, long permissionId)
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
