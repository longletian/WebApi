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
        private readonly IFreeSql freeSql;
        protected BaseEntityRepository(IFreeSql freeSql) : base(freeSql, null, null)
        {
            this.freeSql = freeSql;
        }
  
        public Task<int> DeleteAsync(TEntity entity)
        {
            //默认根据实体里面的id进行删除
            return this.freeSql.Delete<TEntity>().Where(entity).ExecuteAffrowsAsync();
        }

        public Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            return this.freeSql.Delete<TEntity>().Where(entities).ExecuteAffrowsAsync();
        }

        public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> condition)
        {
            return this.freeSql.Select<TEntity>().Where(condition).ToDelete().ExecuteAffrowsAsync();
        }

        public Task<int> InsertAsync(TEntity entity)
        {
            return this.freeSql.Insert<TEntity>(entity).ExecuteAffrowsAsync();
        }

        public long InsertIdentityId(TEntity entity)
        {
            return Convert.ToInt64(this.freeSql.Insert<TEntity>(entity).InsertIdentity());
        }

        public Task InsertPgCopy(IEnumerable<TEntity> entities)
        {
            return this.freeSql.Insert<TEntity>(entities).ExecutePgCopyAsync();
        }

        public Task<int> InsertAsync(IEnumerable<TEntity> entities)
        {
            return this.freeSql.Insert<TEntity>(entities).ExecuteAffrowsAsync();
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            return this.freeSql.Update<TEntity>(entity).ExecuteAffrowsAsync();
        }

        public Task<int> UpdateAsync(IEnumerable<TEntity> entities)
        {
            return this.freeSql.Update<TEntity>().Where(entities).ExecuteAffrowsAsync();
        }

        public TEntity FindEntity(object KeyValue)
        {
            return this.freeSql.Select<TEntity>().WhereDynamic(KeyValue).ToOne();
        }

        public TEntity FindEntity(Expression<Func<TEntity, bool>> condition)
        {
            return this.freeSql.Select<TEntity>().Where(condition).ToOne();
        }

        public TEntity FindEntity(string strSql, Dictionary<string, string> dbParameter = null)
        {
            return this.freeSql.Select<TEntity>().WithSql(strSql, dbParameter).ToOne();
        }

        public IEnumerable<TEntity> FindList()
        {
            return this.freeSql.Select<TEntity>().ToList();
        }

        public IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> condition)
        {
            return this.freeSql.Select<TEntity>().WhereDynamic(condition).ToList();
        }

        public IEnumerable<TEntity> FindList(string strSql)
        {
            return this.freeSql.Select<TEntity>().WithSql(strSql).ToList();
        }

        public IEnumerable<TEntity> FindList(string strSql, object dbParameter)
        {
            return this.freeSql.Select<TEntity>().WithSql(strSql, dbParameter).ToList();
        }

        public IEnumerable<TEntity> FindList(string orderField, int pageSize, int pageIndex)
        {
            return this.freeSql.Select<TEntity>().Page(pageIndex, pageSize).OrderBy(orderField).ToList();
        }

        public IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> condition, string orderField, int pageSize, int pageIndex, out long total)
        {
            var list = this.freeSql.Select<TEntity>().Page(pageIndex, pageSize).OrderBy(orderField).Where(condition).ToList();
            total = list.Count;
            return list;
        }

        public IEnumerable<TEntity> FindList(string strSql, string orderField, int pageSize, int pageIndex, out long total)
        {
            var list = this.freeSql.Select<TEntity>().Page(pageIndex, pageSize).OrderBy(orderField).WithSql(strSql).ToList();
            total = list.Count;
            return list;
        }

        public IEnumerable<TEntity> FindList(string strSql, string orderField, int pageSize, int pageIndex, out int total, Dictionary<string, string> dict = null)
        {
            var list = this.freeSql.Select<TEntity>().Page(pageIndex, pageSize).OrderBy(orderField).WithSql(strSql, dict).ToList();
            total = list.Count;
            return list;
        }

        public object FindObject(string strSql)
        {
            return this.freeSql.Select<object>().WithSql(strSql);
        }

        public Task<int> UpdateAsync(string sql)
        {
            return this.freeSql.Select<TEntity>().WithSql(sql).ToUpdate().ExecuteAffrowsAsync();
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
