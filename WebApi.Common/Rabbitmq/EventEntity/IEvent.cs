using MediatR;
using System;

namespace WebApi.Common
{
    // 事件总线实现发布/订阅功能，首先定义IEvent/IEventHandler，
    // 事件模型
    public interface IEvent: INotification
    {
    }
}
