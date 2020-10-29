using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace WebApi.Repository.Base.Unitwork
{
    public  interface IUnitworkRepository:IDisposable
    {
        #region 事务
       
        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();

        /// <summary>
        /// 保存
        /// </summary>
        void SaveChange();

        /// <summary>
        /// 保存(异步)
        /// </summary>
       Task SaveChangeAsync();

        /// <summary>
        /// 开启事务 一般用于dapper
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();
        #endregion
    }
}
