using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IBaseService<TEntity> where TEntity : class, new()
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
        IEnumerable<TEntity> FindList(string strSql, string orderField, int pageSize, int pageIndex, out long total, Dictionary<string, string> dict = null);
        #endregion

        #region
        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        object FindObject(string strSql);

        #endregion
    }
}
