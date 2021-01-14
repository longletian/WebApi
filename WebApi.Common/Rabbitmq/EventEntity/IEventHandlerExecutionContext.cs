
namespace WebApi.Common
{
   public  interface IEventHandlerExecutionContext
    {
        void RegisterHandler();

        void HandleEventAsync();

        bool HandlerRegistered<TEvent>(TEvent @event) where TEvent : IEvent;

    }
}
