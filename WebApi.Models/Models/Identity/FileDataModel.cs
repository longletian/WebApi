using FreeSql.DataAnnotations;
using WebApi.Models;

namespace WebApi.Model
{
    /// <summary>
    /// 数据权限
    /// </summary>
    [Table(Name = "case_filedata")]
    public class FileDataModel : FullEntity
    {
        [Column(StringLength = 300)]
        public string FileName { get; set; }
        public short? FileType { get; set; }
        public long? FileSize { get; set; }
        [Column(StringLength = 500)]
        public string FilePath { get; set; }
        [Column(StringLength = 40)]
        public string FileMd5 { get; set; }
    }
}
