using System;

namespace WebApi.Models
{
    #region 创建实体
    public interface ICreateAduitEntity
    {
        /// <summary>
        /// 创建者ID
        /// </summary>
        long CreateId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? GmtCreate { get; set; }

    }
    #endregion

    #region 修改实体
    public interface IUpdateAuditEntity
    {
        /// <summary>
        /// 最后修改人Id
        /// </summary>
        long? UpdateId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? UpdateTime { get; set; }
    }
    #endregion

    #region 删除接口
    public interface IDeleteAduitEntity
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        long? DeleteId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeleteTime { get; set; }
    }
    #endregion

    /// <summary>
    /// 根实体
    /// </summary>
    public interface IEntity : IEntity<long>
    {

    }
    /// <summary>
    /// 根实体根据Key
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
