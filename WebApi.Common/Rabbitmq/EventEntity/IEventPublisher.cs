using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    /// <summary>
    ///  定义发布接口
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default(CancellationToken))
            where TEvent : IEvent;
    }
}
