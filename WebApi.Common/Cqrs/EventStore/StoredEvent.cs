using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Common
{
    public class StoredEvent : Event
    {
        public StoredEvent(Event theEvent, string data)
        {
            AggregateId = theEvent.AggregateId;
            MessageType = theEvent.MessageType;
            Data = data;
        }
        // 存储的数据
        public string Data { get; private set; }
    }
}
