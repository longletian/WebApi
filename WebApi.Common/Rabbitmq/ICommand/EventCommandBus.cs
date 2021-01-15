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
        public Task<TResponse> Send<TResponse>(IEventCommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送事件(单播)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Send(IEventCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
