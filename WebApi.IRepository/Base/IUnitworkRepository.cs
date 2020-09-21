using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WebApi.IRepository.Base
{
  public  interface IUnitworkRepository:IDisposable
    {
        #region 事务
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// 获取连接查询
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        /// <summary>
        /// 保存
        /// </summary>
        void SaveChange();
        #endregion
    }
}
