using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 接口管理类
    /// </summary>
    [Table(Name = "case_api")]
    public class ApiEntity : FullEntity
    {
        public string ApiName { get; set; }

        public string LinkUrl { get; set; }

        public string ApiRemark { get; set; }

        public int ApiType { get; set; }

        public bool IsEnable { get; set; }

        public bool IsAuth { get; set; }
    }
}
