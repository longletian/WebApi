using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public interface IEventCommandBus
    {
        Task<TResponse> Send<TResponse>(IEventCommand<TResponse> command, CancellationToken cancellationToken = default(CancellationToken));

        Task Send(IEventCommand command, CancellationToken cancellationToken = default(CancellationToken));
    }
}
