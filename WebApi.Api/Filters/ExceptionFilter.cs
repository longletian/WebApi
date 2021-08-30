
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WebApi.Models;

namespace WebApi.Api.Filters
{
    /// <summary>
    /// IExceptionFilter接口，接口只提供了一个方法OnException，主要的参数为ExceptionContext类，
    /// 基本上就是通过ExceptionContext类来获取相关错误信息，以及进行对应的HTTP请求和响应操作。
    /// FilterAttribute抽象类,可以作为特性使用的全局操作过滤器
    /// 1、Filter过滤器是基于当前Http请求的，也就是接口层面的，颗粒度比较大；
    /// 2、而AOP是基于服务切面的，是 Service 层的请求，颗粒度比较小；
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {

        private readonly IWebHostEnvironment environment;
        private readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(IWebHostEnvironment _environment, ILogger<ExceptionFilter> logger)
        {
            environment = _environment;
            this.logger = logger;
        }

        /// <summary>
        /// 实现IExceptionFilter的OnException方法
        /// <param name="context">异常上下文</param>
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            var response = new ResponseData();
            response.Message = context.Exception.Message;//错误信息
            if (environment.IsDevelopment())
            {
                response.Message = context.Exception.StackTrace;
            }
            response.MsgCode = 500;

            context.Result = new BadRequestObjectResult(response);

            this.logger.LogError(response.Message, WriteLog(response.Message, context.Exception));
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }

    }
}
