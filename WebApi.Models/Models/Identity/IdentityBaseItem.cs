using FreeSql.DataAnnotations;
using System;

namespace WebApi.Models
{
    [Table(Name = "case_dict_item")]
    public class IdentityBaseItem : Entity<long>
    {
        public string DictTypeValue { get; set; }
        public string DictTypeText { get; set; }
        public string DictTypeRemark { get; set; }
        public int DictTypeSortNum { get; set; }
    }
}
