
namespace WebApi.Common
{
    /// <summary>
    /// EventBus实现发布/订阅器，并在事件发布的同时通知相应的事件处理器进行相关处理。
    /// </summary>
    public interface IEventBus : IEventSubscriber, IEventPublisher
    {

    }
}
