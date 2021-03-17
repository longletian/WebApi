using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Enums;

namespace WebApi.Api.Common.Middlewares
{
    /// <summary>
    /// 定义请求管道的中间件
    /// </summary>
    public class LogMiddleware
    {
        private RequestDelegate next;
        private readonly IWebHostEnvironment environment;
        public LogMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            this.next = next;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                Log.Information("开始触发中间件");
                await next(context); //调用管道执行下一个中间件

            }
            catch (Exception ex)
            {
                try
                {
                    await HandlerExceptionAsync(context, ex);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message, "处理异常再出异常");
                }
            }
        }

        private async Task HandlerExceptionAsync(HttpContext context, Exception ex)
        {
            //日志分为业务异常
            //数据库异常
            if (ex != null)
            {
                var messageEx = $"【异常信息】：{ex.Message}\r\n+{ex.StackTrace}\r\n";
                Log.Error(messageEx);
                await JsonHandle(context, ex.Message, ErrorCode.UnknownError, 500);
            }
            else
            {
                //判断是不是开发环境
                if (environment.IsDevelopment())
                {

                }
                else
                {
                    await JsonHandle(context, "服务器正忙，请稍后再试!", ErrorCode.UnknownError, 500);
                }
            }
        }

        /// <summary>
        /// 处理方式：返回Json格式
        /// </summary>
        /// <returns></returns>
        private async Task JsonHandle(HttpContext context, string errorMsg, ErrorCode errorCode, int statusCode)
        {
            ResponseData apiResponse = new ResponseData()
            {
                Message = errorMsg,
                MsgCode =Convert.ToInt32(errorCode),
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(apiResponse.ToString(), Encoding.UTF8); ;
        }
    }
}
