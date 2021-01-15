using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public class MongoStore : IMongoStore
    {
        public Task EventSave(StreamState stream)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StreamState>> GetEvents(Guid aggregateId, int? version = null, DateTime? createdUtc = null)
        {
            throw new NotImplementedException();
        }
    }
}
