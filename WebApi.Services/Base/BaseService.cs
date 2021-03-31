
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Repository;

namespace WebApi.Services
{

    public class BaseService<T> : IBaseService<T> where T : class, new()
    {
        //通过在子类的构造函数中注入，这里是基类，不用构造函数
        public IBaseEntityRepository<T> baseRepository;

        #region 封装方法
        public Task<int> DeleteAsync(T entity)
        {
            return baseRepository.DeleteAsync(entity);
        }

        public Task<int> DeleteAsync(IEnumerable<T> entities)
        {
            return baseRepository.DeleteAsync(entities);
        }

        public Task<int> DeleteAsync(Expression<Func<T, bool>> condition)
        {
            return baseRepository.DeleteAsync(condition);
        }

        public T FindEntity(object KeyValue)
        {
            return baseRepository.FindEntity(KeyValue);
        }

        public T FindEntity(Expression<Func<T, bool>> condition)
        {
            return baseRepository.FindEntity(condition);
        }

        public T FindEntity(string strSql, Dictionary<string, string> dbParameter = null)
        {
            return baseRepository.FindEntity(strSql, dbParameter);
        }

        public IEnumerable<T> FindList()
        {
            return baseRepository.FindList();
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

        public IEnumerable<T> FindList(string orderField, int pageSize, int pageIndex)
        {
            return baseRepository.FindList(orderField, pageSize, pageIndex);
        }

        public IEnumerable<T> FindList(Expression<Func<T, bool>> condition, string orderField, int pageSize, int pageIndex, out long total)
        {
            return baseRepository.FindList(condition, orderField, pageSize, pageIndex, out total);
        }

        public IEnumerable<T> FindList(string strSql, string orderField, int pageSize, int pageIndex, out long total)
        {
            return baseRepository.FindList(strSql, orderField, pageSize, pageIndex, out total);
        }

        public IEnumerable<T> FindList(string strSql, string orderField, int pageSize, int pageIndex, out int total, Dictionary<string, string> dict = null)
        {
            return baseRepository.FindList(strSql, orderField, pageSize, pageIndex, out total, dict);
        }

        public object FindObject(string strSql)
        {
            return baseRepository.FindObject(strSql);
        }

        public Task<int> InsertAsync(T entity)
        {
            return baseRepository.InsertAsync(entity);
        }

        public Task<int> InsertAsync(IEnumerable<T> entities)
        {
            return baseRepository.InsertAsync(entities);
        }

        public long InsertIdentityId(T entity)
        {
            return baseRepository.InsertIdentityId(entity);
        }

        public Task InsertPgCopy(IEnumerable<T> entities)
        {
            return baseRepository.InsertPgCopy(entities);
        }

        public Task<int> UpdateAsync(T entity)
        {
            return baseRepository.UpdateAsync(entity);
        }

        public Task<int> UpdateAsync(IEnumerable<T> entities)
        {
            return baseRepository.UpdateAsync(entities);
        }
        #endregion
    }
}
