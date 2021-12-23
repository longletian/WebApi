using FreeSql.DataAnnotations;
using WebApi.Models;

namespace WebApi.Model
{
    /// <summary>
    /// 数据权限(文件操作方面的)
    /// </summary>
    [Table(Name = "sys_filedata")]
    public class FileDataEntity : FullEntity
    {
        [Column(StringLength = 300)]
        public string Name { get; set; }
        public short? Type { get; set; }
        public long? Size { get; set; }
        [Column(StringLength = 500)]
        public string FilePath { get; set; }
        [Column(StringLength = 40)]
        public string FileMd5 { get; set; }
    }
}
