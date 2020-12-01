using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
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
        /// 开启事务,使用efcore 
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();


        /// <summary>
        /// 获取dbconnextion
        /// </summary>
        /// <returns></returns>
        IDbConnection GetDbConnection();


        /// <summary>
        /// 开始事务,一般用于dapper
        /// </summary>
        /// <returns></returns>
        IDbTransaction GetDbTransaction();
        #endregion
    }
}
