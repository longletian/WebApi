using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    /// <summary>
    /// 事件模型和命令模型区别是  事件只需要发布和订阅 而命令则需要发送
    /// </summary>
    public interface IEventCommandBus
    {
        Task<TResponse> Send<TResponse>(IEventCommand<TResponse> command, CancellationToken cancellationToken = default(CancellationToken));

        Task Send(IEventCommand command, CancellationToken cancellationToken = default(CancellationToken));
    }
}
