using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    /// <summary>
    /// 注册事件
    /// </summary>
    public class EventHandlerExecutionContext : IEventHandlerExecutionContext
    {
        public Task HandleMessageAsync(IEvent @event, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool HandlerRegistered<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler
        {
            throw new NotImplementedException();
        }

        public bool HandlerRegistered(Type messageType, Type messageHandlerType)
        {
            throw new NotImplementedException();
        }

        public void RegisterHandler<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler
        {
            throw new NotImplementedException();
        }

        public void RegisterHandler(Type messageType, Type messageHandlerType)
        {
            throw new NotImplementedException();
        }
    }
}
