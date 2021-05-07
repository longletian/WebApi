using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Common.AOP
{
    /// <summary>
    /// aop切面编程（自定义拦截器）
    /// </summary>
    public class LogAop : IInterceptor
    {
        private readonly IHttpContextAccessor accessor;
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<LogAop> logger;
        public LogAop(ILogger<LogAop> logger,
            IHttpContextAccessor _accessor,
            IWebHostEnvironment _environment)
        {
            this.logger = logger;
            this.accessor = _accessor;
        }

        /// <summary>
        /// 实例化IInterceptor唯一方法
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            // 获取用户的信息
            string UserName = accessor.HttpContext?.User?.Identity?.Name;
            //记录被拦截方法信息的日志信息
            var dataIntercept = "" +
                $"【当前操作用户】：{ UserName} \r\n" +
                $"【当前执行方法】：{ invocation.Method.Name} \r\n" +
                $"【携带的参数有】： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";
            try
            {
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                LogEx(ex, dataIntercept);
                invocation.ReturnValue = GetParamsErrorValueAsync((dynamic)invocation.ReturnValue, ex);
            }
        }

        private void LogEx(Exception ex, string dataIntercept)
        {
            if (ex != null)
            {
                var messageEx = dataIntercept + $"【异常信息】：{ex.Message}\r\n+{ex.StackTrace}\r\n";
                logger.LogError(messageEx);
            }
        }

        private async Task<BaseResponse<T>> GetParamsErrorValueAsync<T>(BaseResponse<T> result, Exception ex)
        {
            await Task.CompletedTask;
            return new BaseResponse<T> { Code = 500, Message = ex.Message };
        }

        /// <summary>
        /// 先判断是否是异步方法
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }
    }
}
