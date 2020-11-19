
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.Repository.Base;

namespace WebApi.Services.Base
{
   public class BaseService<T> :IBaseService<T> where T:class,new ()
    {
        public IBaseRepository<T> baseRepository;

        public void Delete(T entity)
        {
            baseRepository.Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            baseRepository.Delete(entities);
        }

        public void Delete(Expression<Func<T, bool>> condition)
        {
            baseRepository.Delete(condition);
        }

        public Task DeleteAsync(T entity)
        {
            return baseRepository.DeleteAsync(entity);
        }

        public Task DeleteAsync(IEnumerable<T> entities)
        {
            return baseRepository.DeleteAsync(entities);
        }

        public Task DeleteAsync(Expression<Func<T, bool>> condition)
        {
            return baseRepository.DeleteAsync(condition);
        }

        public T ExecuteByProc(string procName)
        {
            return baseRepository.ExecuteByProc(procName);
        }

        public T ExecuteByProc(string procName, object dbParamenter)
        {
            return baseRepository.ExecuteByProc(procName, dbParamenter);
        }

        public int ExecuteBySql(string sql)
        {
            return baseRepository.ExecuteBySql(sql);
        }

        public int ExecuteBySql(string sql, object dbParamenter)
        {
            return baseRepository.ExecuteBySql(sql, dbParamenter);
        }

        public T FindEntity(object KeyValue)
        {
            return baseRepository.FindEntity(KeyValue);
        }

        public T FindEntity(Expression<Func<T, bool>> condition)
        {
            return baseRepository.FindEntity(condition);
        }

        public T FindEntity(string strSql, object dbParameter = null)
        {
            return baseRepository.FindEntity(strSql, dbParameter);
        }

        public IEnumerable<T> FindList()
        {
            return baseRepository.FindList();
        }

        public IEnumerable<T> FindList(Func<T, object> orderby)
        {
            return baseRepository.FindList(orderby);
        }

        public IEnumerable<T> FindList(Expression<Func<T, bool>> condition)
        {
            return baseRepository.FindList(condition);
        }

        public IEnumerable<T> FindList(string strSql)
        {
            return baseRepository.FindList(strSql);
        }

        public IEnumerable<T> FindList(string strSql, object dbParameter)
        {
            return baseRepository.FindList(strSql, dbParameter);
        }

        public IEnumerable<T> FindList(string orderField, int pageSize, int pageIndex, out int total)
        {
            return baseRepository.FindList(orderField, pageSize, pageIndex,out total);
        }

        public IEnumerable<T> FindList(Expression<Func<T, bool>> condition, string orderField, int pageSize, int pageIndex, out int total)
        {
            return baseRepository.FindList(condition, orderField, pageSize, pageIndex, out total);
        }

        public IEnumerable<T> FindList(string strSql, string orderField, int pageSize, int pageIndex, out int total)
        {
            return baseRepository.FindList(strSql, orderField, pageSize, pageIndex, out total);
        }

        public IEnumerable<T> FindList(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total)
        {
            return baseRepository.FindList(strSql, dbParameter,orderField, pageSize, pageIndex, out total);
        }

        public object FindObject(string strSql)
        {
            return baseRepository.FindObject(strSql);
        }

        public object FindObject(string strSql, object dbParameter)
        {
            return baseRepository.FindObject(strSql, dbParameter);
        }

        public DataTable FindTable(string strSql)
        {
            return baseRepository.FindTable(strSql);
        }

        public DataTable FindTable(string strSql, object dbParameter)
        {
            return baseRepository.FindTable(strSql,dbParameter);
        }

        public DataTable FindTable(string strSql, string orderField, int pageSize, int pageIndex, out int total)
        {
            return baseRepository.FindTable(strSql, orderField, pageSize,pageIndex,out total);
        }

        public DataTable FindTable(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total)
        {
            return baseRepository.FindTable(strSql, dbParameter,orderField, pageSize, pageIndex, out total);
        }

        public IEnumerable<T> GetDBTable()
        {
            return baseRepository.GetDBTable();
        }

        public IEnumerable<T> GetDBTableFields(string tableName)
        {
            return baseRepository.GetDBTableFields(tableName);
        }

        public void Insert(T entity)
        {
             baseRepository.Insert(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            baseRepository.Insert(entities);
        }

        public Task InsertAsync(T entity)
        {
           return baseRepository.InsertAsync(entity);
        }

        public Task InsertAsync(IEnumerable<T> entities)
        {
            return baseRepository.InsertAsync(entities);
        }

        public IEnumerable<T> QueryByProc(string procName)
        {
            return baseRepository.QueryByProc(procName);
        }

        public IEnumerable<T> QueryByProc(string procName, object dbParameter)
        {
            return baseRepository.QueryByProc(procName, dbParameter);
        }

        public void Update(T entity)
        {
             baseRepository.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
             baseRepository.Update(entities);
        }

        public Task UpdateAsync(T entity)
        {
          return  baseRepository.UpdateAsync(entity);
        }

        public Task UpdateAsync(IEnumerable<T> entities)
        {
            return baseRepository.UpdateAsync(entities);
        }
    }
}
