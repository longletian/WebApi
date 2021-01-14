using System;

namespace WebApi.Common
{
    /// <summary>
    /// 消息事件参数
    /// </summary>
    public class EventProcessedEventArgs : EventArgs
    {
        public IEvent Event { get; }

        public EventProcessedEventArgs(IEvent @event)
        {
            Event = @event;
        }
    }
}
