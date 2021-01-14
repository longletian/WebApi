using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Common
{
    /// <summary>
    /// 定义事件模型
    /// </summary>
    public abstract class Event : IEvent
    {
        public virtual Guid Id { get; } = Guid.NewGuid();

        public virtual DateTime CreatedUtc { get; } = DateTime.UtcNow;
    }
}
