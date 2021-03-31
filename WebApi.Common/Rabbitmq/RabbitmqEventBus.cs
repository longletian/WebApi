
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public class RabbitmqEventBus : BaseEventBus
    {
        private readonly ILogger<RabbitmqEventBus> logger;
        private readonly IConnection connection;
        private readonly IModel channel;

        public RabbitmqEventBus(IConnection connection, IModel channel, ILogger<RabbitmqEventBus> logger, IEventHandlerExecutionContext context) : base(context)
        {
            this.connection = connection;
            this.channel = channel;
            this.logger = logger;
        }

        public override Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        {
            //this.channel.BasicPublish(@event);
            throw new System.NotImplementedException();
        }

        public override void Subscribe<TEvent, TEventHandler>()
        {
            throw new System.NotImplementedException();
        }
    }
}
