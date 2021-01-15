using System;

namespace WebApi.Common
{
    public class StreamState
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime CreatedUtc { get; private set; } = DateTime.UtcNow;
        public Guid AggregateId { get; set; }
        public string StreamName { get; set; }
        public string EventType { get; set; }
        public string Data { get; set; }
    }
}
