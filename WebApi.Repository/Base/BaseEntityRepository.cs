using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            return this.freeSql.Select<TEntity>().WhereDynamic(condition).Count(out total).Page(pageindex, pagesize).ToList();
        }

        public IEnumerable<TEntity> FindList(string strsql, string orderfield, int pagesize, int pageindex, out long total)
        {
            return this.freeSql.Select<TEntity>().WithSql(strsql).Count(out total).Page(pageindex, pagesize).OrderBy(orderfield).ToList();
        }

        public IEnumerable<TEntity> FindList(string strsql, string orderfield, int pagesize, int pageindex, out long total, Dictionary<string, string> dict = null)
        {

            return this.freeSql.Select<TEntity>().WithSql(strsql, dict).Count(out total).Page(pageindex, pagesize).OrderBy(orderfield).ToList();
        }

        public object FindObject(string strsql)
        {
            return this.freeSql.Ado.ExecuteScalar(strsql);
        }

        public Task<int> UpdateAsync(string sql)
        {
            return this.freeSql.Select<TEntity>().WithSql(sql).ToUpdate().ExecuteAffrowsAsync();
        }

        public int ExecuteBySql(string sql)
        {
            return this.freeSql.Ado.ExecuteNonQuery(sql);
        }

        public int ExecuteBySql(string sql, object dbParamenter)
        {
            return this.freeSql.Ado.ExecuteNonQuery(sql, dbParamenter);
        }

        public TEntity ExecuteByProc(string procName)
        {
            return this.freeSql.Ado.Query<TEntity>(CommandType.StoredProcedure, procName).FirstOrDefault();
        }

        public TEntity ExecuteByProc(string procName, object dbParamenter)
        {
            return this.freeSql.Ado.Query<TEntity>(CommandType.StoredProcedure, procName, null).FirstOrDefault();
        }

        public IEnumerable<TEntity> QueryByProc(string procName)
        {
            return this.freeSql.Ado.Query<TEntity>(CommandType.StoredProcedure, procName);
        }

        public IEnumerable<TEntity> QueryByProc(string procName, object dbParameter)
        {
            throw new NotImplementedException();
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
