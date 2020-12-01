//using Microsoft.EntityFrameworkCore;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace WebApi.Repository.Base.Dapper

{
    public interface IDapperRepository : IDisposable
    {
        #region 执行sql语句
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteBySql(string sql);

        /// <summary>
        /// 执行语句带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParamenter"></param>
        /// <returns></returns>
        int ExecuteBySql(string sql, params object[]  dbParamenter);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        T ExecuteByProc<T>(string procName);

        /// <summary>
        /// 执行存储过程返回一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="dbParamenter"></param>
        /// <returns></returns>
        T ExecuteByProc<T>(string procName, object dbParamenter);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        List<T> QueryByProc<T>(string procName);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        List<T> QueryByProc<T>(string procName, object dbParameter);

        #endregion

        #region 对象实体 查询
  
        /// <summary>
        /// 查找一个实体（根据sql）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        T FindEntity<T>(string strSql, object dbParameter = null) where T : class, new();

        /// <summary>
        /// 查询列表根据sql语句
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        List<T> FindList<T>(string strSql);

        List<T> FindList<T>(string strSql, object dbParameter = null) where T : class, new();

        /// <summary>
        /// 查询列表根据sql语句(带参数)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        List<T> FindList<T>(string strSql, DynamicParameters dbParameter=null);
        /// <summary>
        /// 查询列表(分页)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="orderField">排序字段</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总共数据条数</param>
        /// <returns></returns>
        List<T> FindList<T>(string orderField, int pageSize, int pageIndex, out int total) where T : class, new();
        /// <summary>
        /// 查询列表(分页)带表达式条件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总共数据条数</param>
        /// <returns></returns>
        List<T> FindList<T>(Expression<Func<T, bool>> condition, string orderField, int pageSize, int pageIndex, out int total) where T : class, new();

        /// <summary>
        /// 查询列表(分页)根据sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总共数据条数</param>
        /// <returns></returns>
        List<T> FindList<T>(string strSql, string orderField, int pageSize, int pageIndex, out int total) where T : class;
        /// <summary>
        /// 查询列表(分页)根据sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总共数据条数</param>
        /// <returns></returns>
        List<T> FindList<T>(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total) where T : class;
        #endregion

        #region 事务处理
        IDbConnection GetDbConnection();

        IDbTransaction GetDbTransaction();
        #endregion
    }
}
