using Grpc.Core;
using GrpcService;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApi.Api
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 重载方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context">可以获取gRPC API 提供对某些 HTTP/2 消息数据（如方法、主机、标头和尾部）的访问权限</param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Saying hello to {Name}", request.Name);
            var httpContext = context.GetHttpContext();
            //var clientCertificate = httpContext.Connection.ClientCertificate;
            return Task.FromResult(new HelloReply
            {
                Message = "Hello "
            });
        }
        /// <summary>
        /// 服务器流式处理(以参数的形式获取请求消息,由于可以将多个消息流式传输回调用方，
        /// 因此可使用 responseStream.WriteAsync 发送响应消息)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task StreamingFromServer(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            //服务器流式处理方法启动后，客户端无法发送其他消息或数据
            //当发生取消时，客户端会将信号发送到服务器
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new HelloReply());
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }

        /// <summary>
        /// 客户端流式处理(在该方法没有接收消息的情况下启动)
        /// </summary>
        /// <param name="requestStream">用于从客户端读取消息</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async  override Task<HelloReply> StreamingFromClient(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            //while (await requestStream.MoveNext())
            //{
            //    var message = requestStream.Current;
            //    // ...
            //}
            // requestStream.ReadAllAsync 扩展方法读取请求数据流中的所有消息
            await foreach (var message in requestStream.ReadAllAsync())
            {
                // ...
            }

            return new HelloReply();
        }

        /// <summary>
        /// 双向流式处理(在该方法没有接收到消息的情况下启动,客户端和服务可在任何时间互相发送消息)
        /// </summary>
        /// <param name="requestStream">从客户端读取消息</param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task StreamingBothWays(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var readTask = Task.Run(async () =>
            {
                await foreach (var message in requestStream.ReadAllAsync())
                {
                    // Process request.
                }
            });

            // Send responses until the client signals that it is complete.
            while (!readTask.IsCompleted)
            {
                await responseStream.WriteAsync(new HelloReply());
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }

    }
}
