using System;

namespace WebApi.Models
{
    public interface ITrack
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        long CreateorId { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        long UpdateorId { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDelete { get; set; }
    }
}
