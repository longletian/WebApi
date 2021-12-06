using FreeSql.DataAnnotations;
using System;

namespace WebApi.Models
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table(Name = "case_dict_type")]
    public class IdentityBaseType : Entity<long>, IDeleteAduitEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public int SrotNum { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeleteId { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
