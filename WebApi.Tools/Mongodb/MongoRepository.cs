using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Tools
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class
    {
        private readonly IMongoDatabase db;
        public MongoRepository(IOptions<MongoOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            this.db = client.GetDatabase(options.Value.Database);
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
