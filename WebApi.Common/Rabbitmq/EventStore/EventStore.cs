
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public class EventStore : IEventStore
    {
        public Task EventSave(IEvent @event, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StreamState>> GetEvents(Guid aggregateId, int? version = null, DateTime? createdUtc = null)
        {
            throw new NotImplementedException();
        }
    }
}
