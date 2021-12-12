using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
   public  interface IEventHandlerExecutionContext
    {

        /// <summary>
        /// 对接收到的事件消息进行处理
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleMessageAsync(IEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// 判断事件处理器是否已经注册
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageHandlerType"></param>
        bool HandlerRegistered(Type messageType, Type messageHandlerType);

        /// <summary>
        /// 注册事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        void RegisterHandler<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler;

        void RegisterHandler(Type messageType, Type messageHandlerType);

        bool HandlerRegistered<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler;

    }
}
