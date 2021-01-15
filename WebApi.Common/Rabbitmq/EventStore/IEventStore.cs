using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        ///  添加事件
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task EventSave(IEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取事件
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="version"></param>
        /// <param name="createdUtc"></param>
        /// <returns></returns>
        Task<IEnumerable<StreamState>> GetEvents(Guid aggregateId, int? version = null, DateTime? createdUtc = null);
    }
}
