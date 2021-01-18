using System;

namespace WebApi.Common
{
    public class EventMessage
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }
        public string EventName { get; set; }

        protected EventMessage()
        {
            MessageType = GetType().Name;
            EventName = "common-event";
        }
    }
}
