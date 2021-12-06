using FreeSql.DataAnnotations;
using System;

namespace WebApi.Models
{
    [Table(Name = "case_dict_item")]
    public class DictBaseItemEntity : Entity<long>, IDeleteAduitEntity
    {
        public int ItemTypeId { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string Remark { get; set; }
        public int SortNum { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeleteId { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
