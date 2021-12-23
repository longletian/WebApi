using FreeSql.DataAnnotations;
using System;

namespace WebApi.Models
{
    [Table(Name = "sys_dict_item")]
    public class DictBaseItemEntity : Entity<long>, IDeleteAduitEntity
    {
        public int TypeId { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string Remark { get; set; }
        public int? Sort { get; set; }
        public long? DeleteId { get; set; }
        public DateTime? GmtDeleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
