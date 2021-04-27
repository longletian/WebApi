using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Repository
{
    /// <summary>
    /// 继承框架中得BaseRepository并实现了IBaseEntityRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityKey"></typeparam>
    public abstract class BaseEntityRepository<TEntity, TKey> : BaseRepository<TEntity, TKey> ,IBaseEntityRepository<TEntity, TKey> where TEntity : class
    {
        #region MyRegion
        private readonly IFreeSql freeSql;
        protected BaseEntityRepository(UnitOfWorkManager unitofworkmanager) : base(unitofworkmanager?.Orm,null)
        {
            this.freeSql = unitofworkmanager?.Orm;
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

        public TEntity FindEntity(object keyvalue)
        {
            return this.freeSql.Select<TEntity>().WhereDynamic(keyvalue).ToOne();
        }

        public TEntity FindEntity(Expression<Func<TEntity, bool>> condition)
        {
            return this.freeSql.Select<TEntity>().Where(condition).ToOne();
        }

        public TEntity FindEntity(string strsql, Dictionary<string, string> dbparameter = null)
        {
            return this.freeSql.Select<TEntity>().WithSql(strsql, dbparameter).ToOne();
        }

        public IEnumerable<TEntity> FindList()
        {
            return this.freeSql.Select<TEntity>().ToList();
        }

        public IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> condition)
        {
            return this.freeSql.Select<TEntity>().WhereDynamic(condition).ToList();
        }

        public IEnumerable<TEntity> FindList(string strsql)
        {
            return this.freeSql.Select<TEntity>().WithSql(strsql).ToList();
        }

        public IEnumerable<TEntity> FindList(string strsql, object dbparameter)
        {
            return this.freeSql.Select<TEntity>().WithSql(strsql, dbparameter).ToList();
        }

        public IEnumerable<TEntity> FindList(string orderfield, int pagesize, int pageindex)
        {
            return this.freeSql.Select<TEntity>().Page(pageindex, pagesize).OrderBy(orderfield).ToList();
        }

        public IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> condition, string orderfield, int pagesize, int pageindex, out long total)
        {
            var list = this.freeSql.Select<TEntity>().Page(pageindex, pagesize).OrderBy(orderfield).Where(condition).ToList();
            total = list.Count;
            return list;
        }

        public IEnumerable<TEntity> FindList(string strsql, string orderfield, int pagesize, int pageindex, out long total)
        {
            var list = this.freeSql.Select<TEntity>().Page(pageindex, pagesize).OrderBy(orderfield).WithSql(strsql).ToList();
            total = list.Count;
            return list;
        }

        public IEnumerable<TEntity> FindList(string strsql, string orderfield, int pagesize, int pageindex, out int total, Dictionary<string, string> dict = null)
        {
            var list = this.freeSql.Select<TEntity>().Page(pageindex, pagesize).OrderBy(orderfield).WithSql(strsql, dict).ToList();
            total = list.Count;
            return list;
        }

        public object FindObject(string strsql)
        {
            return this.freeSql.Select<object>().WithSql(strsql);
        }

        public Task<int> UpdateAsync(string sql)
        {
            return this.freeSql.Select<TEntity>().WithSql(sql).ToUpdate().ExecuteAffrowsAsync();
        }
        #endregion

    }


    public abstract class BaseEntityRepository<TEntity> : BaseEntityRepository<TEntity, long> where TEntity : class, new()
    {
        public BaseEntityRepository(UnitOfWorkManager unitofworkmanager) : base(unitofworkmanager)
        {
            unitofworkmanager.Binding(this);
        }
    }
}
