using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Repository
{

    /// <summary>
    /// 继承freesql自封装的IBaseRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseEntityRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region 对象实体 添加、修改、删除

        Task<int> InsertAsync(TEntity entity);

        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 返回增加的id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        long InsertIdentityId(TEntity entity);

        /// <summary>
        /// PostgreSQL 特有的功能，执行 Copy 批量导入数据
        /// </summary>
        /// <returns></returns>
        Task InsertPgCopy(IEnumerable<TEntity> entities);
        Task<int> DeleteAsync(TEntity entity);

        Task<int> DeleteAsync(IEnumerable<TEntity> entities);

        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> condition);

        Task<int> UpdateAsync(TEntity entity);

        Task<int> UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(string sql);

        #endregion

        #region 执行sql语句
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        //int ExecuteBySql(string sql);

        /// <summary>
        /// 执行语句带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParamenter"></param>
        /// <returns></returns>
        //int ExecuteBySql(string sql, object dbParamenter);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        //TEntity ExecuteByProc(string procName);

        /// <summary>
        /// 执行存储过程返回一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="dbParamenter"></param>
        /// <returns></returns>
        //TEntity ExecuteByProc(string procName, object dbParamenter);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        //IEnumerable<TEntity> QueryByProc(string procName);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        //IEnumerable<TEntity> QueryByProc(string procName, object dbParameter);

        #endregion

        #region 对象实体 查询
        /// <summary>
        /// 查找一个实体根据主键
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        TEntity FindEntity(object KeyValue);
        /// <summary>
        /// 查找一个实体（根据表达式）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        TEntity FindEntity(Expression<Func<TEntity, bool>> condition);
        /// <summary>
        /// 查找一个实体（根据sql）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        TEntity FindEntity(string strSql, Dictionary<string, string> dbParameter = null);

        /// <summary>
        /// 查询列表（获取表所有数据）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> FindList();

        /// <summary>
        /// 查询列表根据表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> condition);
        /// <summary>
        /// 查询列表根据sql语句
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindList(string strSql);

        /// <summary>
        /// 查询列表根据sql语句(带参数)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindList(string strSql, object dbParameter);
        /// <summary>
        /// 查询列表(分页)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="orderField">排序字段</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总共数据条数</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindList(string orderField, int pageSize, int pageIndex);
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
        IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> condition, string orderField, int pageSize, int pageIndex, out long total);

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
        IEnumerable<TEntity> FindList(string strSql, string orderField, int pageSize, int pageIndex, out long total);
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
        IEnumerable<TEntity> FindList(string strSql, string orderField, int pageSize, int pageIndex, out int total, Dictionary<string, string> dict = null);
        #endregion

        #region 数据源查询
        ///// <summary>
        ///// 查询数据
        ///// </summary>
        ///// <param name="strSql">sql语句</param>
        ///// <returns></returns>
        //DataTable FindTable(string strSql);
        ///// <summary>
        ///// 查询数据
        ///// </summary>
        ///// <param name="strSql">sql语句</param>
        ///// <param name="dbParameter">参数</param>
        ///// <returns></returns>
        //DataTable FindTable(string strSql, object dbParameter);
        ///// <summary>
        ///// 查询数据
        ///// </summary>
        ///// <param name="strSql">sql语句</param>
        ///// <param name="orderField">排序字段</param>
        ///// <param name="pageSize">每页数据条数</param>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="total">总共数据条数</param>
        ///// <returns></returns>
        //DataTable FindTable(string strSql, string orderField, int pageSize, int pageIndex, out long total);
        ///// <summary>
        ///// 查询数据
        ///// </summary>
        ///// <param name="strSql">sql语句</param>
        ///// <param name="dbParameter">参数</param>
        ///// <param name="orderField">排序字段</param>
        ///// <param name="pageSize">每页数据条数</param>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="total">总共数据条数</param>
        ///// <returns></returns>
        //DataTable FindTable(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out long total);
        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        object FindObject(string strSql);
        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        //object FindObject(string strSql, object dbParameter);
        #endregion
    }

    /// <summary>
    /// 继承freesql自封装的IBaseRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    //public interface IBaseEntityRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    //{
    //}
}
