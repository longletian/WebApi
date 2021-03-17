
using WebApi.Tools.Mongodb;

namespace WebApi.Tools
{
    public class MongoStoreDatabaseSettings: IMongoStoreDatabaseSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string Database { get; set; }
    }
}
