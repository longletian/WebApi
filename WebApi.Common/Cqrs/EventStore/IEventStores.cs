using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public interface IEventStores
    {
        /// <summary>
        /// 返回连接对象
        /// </summary>
        /// <returns></returns>
        IEventStoreConnection GetEventStoreConnection();

        /// <summary>
        ///  添加事件
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task EventSave(Event @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取事件
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="version"></param>
        /// <param name="createdUtc"></param>
        /// <returns></returns>
        Task<List<ResolvedEvent>> GetEvents(Guid aggregateId);
    }
}
