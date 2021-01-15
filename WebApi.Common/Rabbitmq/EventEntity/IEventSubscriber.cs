namespace WebApi.Common
{
    /// <summary>
    /// 定义订阅接口
    /// </summary>
    public interface IEventSubscriber
    {
        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="TEvent">事件模型</typeparam>
        /// <typeparam name="TEventHandler">事件处理</typeparam>
        void Subscribe<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>;

    }
}
