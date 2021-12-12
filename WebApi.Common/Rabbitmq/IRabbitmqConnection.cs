using RabbitMQ.Client;
using System;

namespace WebApi.Common
{
    public interface IRabbitmqConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

    }
}
