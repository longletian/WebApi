using FreeSql.DataAnnotations;
using System;

namespace WebApi.Models
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table(Name = "sys_dict_type")]
    public class DictBaseTypeEntity : Entity<long>, IDeleteAduitEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public int? Sort{ get; set; }
        public long? DeleteId { get; set; }
        public DateTime? GmtDeleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
