using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace WebApi.Api.Filters
{
    /// <summary>
    /// IExceptionFilter接口，接口只提供了一个方法OnException，主要的参数为ExceptionContext类，
    /// 基本上就是通过ExceptionContext类来获取相关错误信息，以及进行对应的HTTP请求和响应操作。
    /// FilterAttribute抽象类,可以作为特性使用的全局操作过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }
        
        /// <summary>
        /// 实现IExceptionFilter的OnException方法
        /// <param name="context">异常上下文</param>
        /// </summary>
        public void OnException(ExceptionContext context)
        {
        }
    }
}
