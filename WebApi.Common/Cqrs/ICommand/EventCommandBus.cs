using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public class EventCommandBus : IEventCommandBus
    {
        private readonly IMediator mediator;
        public EventCommandBus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// 发送事件(多播)
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Send<TResponse>(IEventCommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            return await this.mediator.Send<TResponse>(command, cancellationToken);
        }

        /// <summary>
        /// 发送事件(单播)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Send(IEventCommand command, CancellationToken cancellationToken = default)
        {
            await this.mediator.Send(command, cancellationToken);
        }
    }
}
