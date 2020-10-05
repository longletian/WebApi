using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebApi.Api.Common.Middlewares
{
    /// <summary>
    /// 定义请求管道的中间件
    /// </summary>
    public class LogMiddleware
    {

        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var exception = context.Features.Get<Exception>();

            await _next(context);
        }
    }
}
