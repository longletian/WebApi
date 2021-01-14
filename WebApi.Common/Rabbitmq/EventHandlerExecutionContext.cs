using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Common
{
    public class EventHandlerExecutionContext : IEventHandlerExecutionContext
    {
        public void HandleEventAsync()
        {
            throw new NotImplementedException();
        }

        public bool HandlerRegistered<TEvent>(TEvent @event) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public void RegisterHandler()
        {
            throw new NotImplementedException();
        }
    }
}
