using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Entity : Entity<long>, IEntity
    {

    }

    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateorId { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public long UpdateorId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 添加或更新时生成的值
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdateTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
