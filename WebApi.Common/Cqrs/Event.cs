using Newtonsoft.Json;
using System;

namespace WebApi.Common
{
    /// <summary>
    /// 定义事件模型
    /// </summary>
    public abstract class Event: EventMessage,IEvent
    {
        public Event()
        {
            Id = Guid.NewGuid();
            CreateDateTime = DateTime.UtcNow;
        }

        [JsonConstructor]
        public Event(Guid id,DateTime createDateTime)
        {
            Id = id;
            CreateDateTime = createDateTime;
        }


        [JsonProperty]
        public Guid Id { get; }


        [JsonProperty]
        public DateTime CreateDateTime { get; }
    }
}
