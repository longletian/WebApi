using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApi.Tools.Mongodb;

namespace WebApi.Tools
{
    /// <summary>
    /// 文档仓储类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> collection;
        private readonly IMongoDatabase db;
        public MongoRepository(IMongoStoreDatabaseSettings options)
        {
            var client = new MongoClient(options.ConnectionString);
            this.db = client.GetDatabase(options.Database);
            this.collection = db.GetCollection<TEntity>(options.CollectionName);
        }

        public Task DeleteAsync(TEntity entity)
        {
            //return await this.db.GetCollection<TEntity>(nameof(TEntity))
            //    .DeleteOneAsync();
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> FindList()
        {
            throw new NotImplementedException();
            //return await this.collection.Find<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> FindListWhere(Expression<Func<TEntity, bool>> filter)
        {
            return await this.collection.Find(filter).ToListAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await this.db.GetCollection<TEntity>(nameof(TEntity))
              .InsertOneAsync(entity);
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await this.db.GetCollection<TEntity>(nameof(TEntity))
               .InsertManyAsync(entities);
        }

        public Task UpdateAsync(TEntity entity)
        {
            //var filter = Builders<TEntity>.Filter.Eq("Id", entity.);
            //await this.db.GetCollection<TEntity>(nameof(TEntity))
            //   .UpdateOne(filter:item=> ,);
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
