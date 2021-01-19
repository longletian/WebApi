using Grpc.Core;
using GrpcService;
using Microsoft.Extensions.Logging;
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
        /// <param name="context">gRPC API 提供对某些 HTTP/2 消息数据（如方法、主机、标头和尾部）的访问权限</param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Saying hello to {Name}", request.Name);
            //var httpContext = context.GetHttpContext();
            //var clientCertificate = httpContext.Connection.ClientCertificate;

            return Task.FromResult(new HelloReply
            {
                Message = "Hello "
            });
        }

    }
}
