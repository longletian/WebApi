using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Common.Middlewares
{
    /// <summary>
    /// IP白名单控制中间件
    /// </summary>
    public class IPControlMiddleware
    {
        private RequestDelegate next;

        public IPControlMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "GET" || context.Request.Method == "POST")
            {
                // 获取请求的远程ip
                var remoteIp = context.Connection.RemoteIpAddress;    //获取远程访问IP
                //string[] ip = _illegalIpList.Split(';');
                var bytes = remoteIp.GetAddressBytes();
                //var badIp = false;
                //foreach (var address in ip)
                //{
                //    var testIp = IPAddress.Parse(address);
                //    if (testIp.GetAddressBytes().SequenceEqual(bytes))
                //    {
                //        badIp = true;
                //        break;    //直接跳出ForEach循环
                //    }
                //}
                //if (badIp)
                //{
                //    context.Response.StatusCode = 401;
                //    return;
                //}
            }
            await next.Invoke(context);
        }

    }
}
