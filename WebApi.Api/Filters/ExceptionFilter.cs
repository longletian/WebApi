using Microsoft.AspNetCore.Mvc.Filters;
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
        public void OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
