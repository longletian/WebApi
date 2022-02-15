using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table(Name = "case_dict_type")]
    public class IdentityBaseType : Entity<long>
    {
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string TypeRemark { get; set; }
    }
}
