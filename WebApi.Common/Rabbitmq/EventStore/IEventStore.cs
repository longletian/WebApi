
namespace WebApi.Common
{ 
    public interface IEventStore
    {
        void SaveEvent<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
