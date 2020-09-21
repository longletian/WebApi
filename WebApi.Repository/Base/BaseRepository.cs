using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApi.IRepository.Base;
using WebApi.Models;
using WebApi.Models.Entity;

namespace WebApi.Repository.Base
{   
    /// <summary>
    /// 基础实体
    /// </summary>
    public class BaseRepository 
    {
        private readonly IFreeSql freeSql;
        public BaseRepository(IFreeSql _freeSql)
        {
            freeSql = _freeSql;
        }
        /// <summary>
        /// 删除实体，返回影响的条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<T>(T entity) where T : class
        {
            return freeSql.Delete<T>().Where(entity).ExecuteAffrows();
        }
        /// <summary>
        /// 删除多个实体，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete<T>(IEnumerable<T> entities) where T : class
        {
            return freeSql.Delete<T>().Where(entities).ExecuteAffrows();
        }

        public int Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public int ExecuteByProc(string procName)
        {
            throw new NotImplementedException();
        }

        public int ExecuteByProc(string procName, object dbParamenter)
        {
            throw new NotImplementedException();
        }

        public T ExecuteByProc<T>(string procName) where T : class
        {
            throw new NotImplementedException();
        }

        public T ExecuteByProc<T>(string procName, object dbParamenter) where T : class
        {
            throw new NotImplementedException();
        }

        public int ExecuteBySql(string sql)
        {
            throw new NotImplementedException();
        }

        public int ExecuteBySql(string sql, object dbParamenter)
        {
            throw new NotImplementedException();
        }

        public T FindEntity<T>(object KeyValue) where T : class
        {
            throw new NotImplementedException();
        }

        public T FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public T FindEntity<T>(string sql, object dbParamenter = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public int FindEntityNum<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public int FindExistEntity<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public int FindExistEntity<T>(Expression<Func<T, bool>> condition) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(Func<T, object> orderby) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string strSql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string strSql, object dbParameter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string orderField, int pageSize, int pageIndex, out int total) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string strSql, string orderField, int pageSize, int pageIndex, out int total) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(Expression<Func<T, bool>> condition, string orderField, int pageSize, int pageIndex, out int total) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public DbConnection GetDbConnection()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, string>> OrderLambda = null, bool isAsc = true) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> IQueryable<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> QueryByProc<T>(string procName) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> QueryByProc<T>(string procName, object dbParameter) where T : class
        {
            throw new NotImplementedException();
        }

        public int Update<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public int Update<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
