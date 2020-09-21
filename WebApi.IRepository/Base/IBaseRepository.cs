using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.IRepository.Base
{
    public  interface IBaseRepository
    {

        /// <summary>
        /// 获取连接上下文
        /// </summary>
        /// <returns></returns>
        DbConnection GetDbConnection();

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
        int ExecuteBySql(string sql, object dbParamenter);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        int ExecuteByProc(string procName);

        /// <summary>
        /// 执行存储过程（带参数）
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="dbParamenter"></param>
        /// <returns></returns>
        int ExecuteByProc(string procName, object dbParamenter);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        T ExecuteByProc<T>(string procName) where T : class;

        /// <summary>
        /// 执行存储过程返回一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="dbParamenter"></param>
        /// <returns></returns>
        T ExecuteByProc<T>(string procName, object dbParamenter) where T:class;

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        IEnumerable<T> QueryByProc<T>(string procName) where T : class;

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        IEnumerable<T> QueryByProc<T>(string procName, object dbParameter) where T : class;

        #endregion

        #region 对象实体 添加、修改、删除

        /// <summary>
        /// 插入实体数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        int Insert<T>(T entity) where T : class;

        /// <summary>
        /// 插入实体数据(异步)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        Task<int> InsertAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量插入实体数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entities">实体数据列表</param>
        /// <returns></returns>
        int Insert<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// 批量插入实体数据（异步）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entities">实体数据列表</param>
        /// <returns></returns>
        Task<int> InsertAsync<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体数据（需要主键赋值）</param>
        /// <returns></returns>
        int Delete<T>(T entity) where T : class;

        /// <summary>
        /// 删除实体数据(异步)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体数据（需要主键赋值）</param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量删除实体数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entities">实体数据列表</param>
        /// <returns></returns>
        int Delete<T>(IEnumerable<T> entities) where T : class;


        /// <summary>
        /// 批量删除实体数据（异步）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entities">实体数据列表</param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// 删除表数据（根据Lambda表达式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        int Delete<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// 删除表数据（根据Lambda表达式）（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        int Update<T>(T entity) where T : class;

        /// <summary>
        /// 更新实体数据（异步）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        Task<int> UpdateAsync<T>(T entity) where T : class;

        /// <summary>
        /// 批量更新实体数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entities">实体数据列表</param>
        /// <returns></returns>
        int Update<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// 批量更新实体数据(异步)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entities">实体数据列表</param>
        /// <returns></returns>
        Task<int> UpdateAsync<T>(IEnumerable<T> entities) where T : class;
        #endregion

        #region 对象实体查询

        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <returns></returns>
        int FindExistEntity<T>() where T : class;

        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <returns></returns>
        int FindExistEntity<T>(Expression<Func<T, bool>> condition) where T : class;

        /// <summary>
        /// 判断查询条件的数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        int  FindEntityNum<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// 查找一个实体根据主键
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        T FindEntity<T>(object KeyValue) where T : class;

        /// <summary>
        /// 查询实体（lamdata表达式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        T FindEntity<T>(Expression<Func<T, bool>> condition) where T : class,new ();


        /// <summary>
        /// 查询实体（lamdata表达式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        T FindEntity<T>(string sql, object dbParamenter = null) where T : class, new();

        /// <summary>
        /// 获取IQueryable集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> IQueryable<T>() where T : class, new();

        /// <summary>
        /// 获取IQueryable表达式(根据表达式)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// 查询列表（获取表所有数据）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        IEnumerable<T> FindList<T>() where T : class, new();

        /// <summary>
        /// 查询列表根据表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        IEnumerable<T> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new();

        /// <summary>
        /// 查询列表（获取表所有数据）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        IEnumerable<T> FindList<T>(Func<T, object> orderby) where T : class, new();

        /// <summary>
        /// 查询列表根据sql语句
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        IEnumerable<T> FindList<T>(string strSql);

        /// <summary>
        /// 查询列表根据sql语句(带参数)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        IEnumerable<T> FindList<T>(string strSql, object dbParameter);

        /// <summary>
        /// 查询列表(分页)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="orderField">排序字段</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总共数据条数</param>
        /// <returns></returns>
        IEnumerable<T> FindList<T>(string orderField, int pageSize, int pageIndex, out int total) where T : class, new();

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
        IEnumerable<T> FindList<T>(string strSql, string orderField, int pageSize, int pageIndex, out int total) where T : class;

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
        IEnumerable<T> FindList<T>(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total) where T : class;

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
        IEnumerable<T> FindList<T>(Expression<Func<T, bool>> condition, string orderField, int pageSize, int pageIndex, out int total) where T : class, new();

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="OrderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        IQueryable<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, string>> OrderLambda = null, bool isAsc = true) where T : class, new();

        #endregion

    }
}
