using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Repository
{
    /// <summary>
    /// 继承框架中得BaseRepository并实现了IBaseEntityRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityKey"></typeparam>
    public class BaseEntityRepository<TEntity, TEntityKey> : BaseRepository<TEntity, TEntityKey>, IBaseEntityRepository<TEntity, TEntityKey> where TEntity : class
    {
        private readonly IFreeSql fsql;
        protected BaseEntityRepository(IFreeSql fsql) : base(fsql, null, null)
        {
            this.fsql = fsql;
        }

        public Task DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public TEntity ExecuteByProc(string procName)
        {
            throw new NotImplementedException();
        }

        public TEntity ExecuteByProc(string procName, object dbParamenter)
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

        public TEntity FindEntity(object KeyValue)
        {
            throw new NotImplementedException();
        }

        public TEntity FindEntity(Expression<Func<TEntity, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public TEntity FindEntity(string strSql, object dbParameter = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList(Func<TEntity, object> orderby)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList(string strSql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList(string strSql, object dbParameter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList(string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> condition, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList(string strSql, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindList(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public object FindObject(string strSql)
        {
            throw new NotImplementedException();
        }

        public object FindObject(string strSql, object dbParameter)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable(string strSql)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable(string strSql, object dbParameter)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable(string strSql, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetDBTable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetDBTableFields(string tableName)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> QueryByProc(string procName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> QueryByProc(string procName, object dbParameter)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
    public class BaseEntityRepository<TEntity> : BaseEntityRepository<TEntity, Guid>, IBaseEntityRepository<TEntity> where TEntity :class
    {
        public BaseEntityRepository(UnitOfWorkManager unitOfWorkManager) : base(unitOfWorkManager?.Orm)
        {
            unitOfWorkManager.Binding(this);
        }
    }
}
