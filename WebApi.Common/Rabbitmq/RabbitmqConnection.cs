using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Net.Http;

namespace WebApi.Common
{
    public class RabbitmqConnection : IRabbitmqConnection
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private object lockObjects = new object();
        private RabbitmqOptions option;
        public RabbitmqConnection(IOptions<RabbitmqOptions> options)
        {
            try
            {
                this.option = options.Value;
                var factory = new ConnectionFactory()
                {
                    HostName = option.HostName,
                    UserName = option.UserName,
                    Password = option.Password,
                    Port = option.Port,
                    VirtualHost= option.VirtualHost,
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                Log.Error("rabbitmq连接异常", ex.Message);
            }
           
        }

        public bool IsConnected
        {
            get 
            {
                return this.connection != null && this.connection.IsOpen;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            return connection.CreateModel();
        }

        public void Dispose()
        {
            try
            {
                this.connection.Close();
                this.connection.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 调用polly异常处理
        /// </summary>
        /// <returns></returns>
        public bool TryConnect()
        {
            Log.Information("rabbitmq 数据连接开始");

            //防止并发调用
            lock (lockObjects) {
                // 自定义异常处置
                var policy =
                    Policy.Handle<HttpRequestException>()
                    .WaitAndRetry(5, retryAction =>
                      TimeSpan.FromSeconds(Math.Pow(2, retryAction)), (ex, time) =>
                      {
                          Log.Warning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                      }
                    );

                policy.Execute(() =>
                {

                });

                return true;
            }
        }
    }
}
