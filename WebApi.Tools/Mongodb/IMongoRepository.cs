using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Tools
{
    public interface IMongoRepository<TEntity> where TEntity :class
    {
        Task InsertAsync(TEntity entity);

        Task InsertAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(IEnumerable<TEntity> entities);

        Task UpdateAsync(TEntity entity);

        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> FindList();

        /// <summary>
        /// 查询数据加条件
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> FindListWhere(Expression<Func<TEntity, bool>> filter);
    }
}
