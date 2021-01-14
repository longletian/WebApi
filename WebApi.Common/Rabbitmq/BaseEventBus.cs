

using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    /// <summary>
    /// 定义抽象类
    /// </summary>
    public abstract class BaseEventBus : IEventBus
    {
        protected readonly IEventHandlerExecutionContext eventHandlerExecutionContext;

        public BaseEventBus(IEventHandlerExecutionContext eventHandlerExecutionContext)
        {
            this.eventHandlerExecutionContext = eventHandlerExecutionContext;
        }

        /// <summary>
        /// 定义抽象方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;

        public abstract void Subscribe<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>;
    }
}
