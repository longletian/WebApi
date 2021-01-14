using System;

namespace WebApi.Common
{
    internal sealed class EventQueue
    {

        public event EventHandler<EventProcessedEventArgs> EventPushed;

        public EventQueue() { }

        /// <summary>
        /// 当事件push时，立即触发OnMessagePushed
        /// </summary>
        /// <param name="event"></param>
        public void Push(IEvent @event)
        {
            OnMessagePushed(new EventProcessedEventArgs(@event));
        }

        /// <summary>
        /// 通知 EventQueue 对象的订阅者，消息已经被派发
        /// </summary>
        /// <param name="e"></param>
        private void OnMessagePushed(EventProcessedEventArgs e) => this.EventPushed?.Invoke(this, e);
    }
}
