using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public sealed class EventBus : BaseEventBus
    {
        private readonly ILogger<EventBus> logger;
        private readonly EventQueue eventQueue = new EventQueue();

        public EventBus(IEventHandlerExecutionContext context, ILogger<EventBus> logger) : base(context)
        {
            this.logger = logger;
            logger.LogInformation($"PassThroughEventBus构造函数调用完成。Hash Code：{this.GetHashCode()}.");

            eventQueue.EventPushed += EventQueue_EventPushed;
        }

        public  override Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => eventQueue.Push(@event));
        }


        public override void Subscribe<TEvent, TEventHandler>()
        {
            if (!this.eventHandlerExecutionContext.HandlerRegistered<TEvent, TEventHandler>())
            {
                this.eventHandlerExecutionContext.RegisterHandler<TEvent, TEventHandler>();
            }
        }

        private async void EventQueue_EventPushed(object sender, EventProcessedEventArgs e)
            => await eventHandlerExecutionContext.HandleMessageAsync(e.Event);

    }
}
