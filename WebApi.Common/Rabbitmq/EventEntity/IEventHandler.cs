

using MediatR;
using System.Threading;
using System.Threading.Tasks;
namespace WebApi.Common
{

    //IEventHandler 定义了事件处理方法
    public interface IEventHandler : ITransientDependency
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 可否处理
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        bool CanHandle(IEvent @event);
    }

    public interface IEventHandler<in T> : INotificationHandler<T>, IEventHandler where T : IEvent
    {

    }
}
