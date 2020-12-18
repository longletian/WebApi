using System;
using WebApi.Models;
using Castle.DynamicProxy;
using System.Threading.Tasks;
using WebApi.Common.BaseHelper.ValidatorHelper;

namespace WebApi.Common.AOP
{
    /// <summary>
    /// 验证aop(autofac代理)
    /// 拦截器 需要实现 IInterceptor接口 Intercept方法
    /// </summary>
    public class ValidatorAop : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var request = invocation.Arguments[0];

            var isValid = request.IsValid(out var message);

            if (!isValid)
            {
                var resultType = invocation.Method.ReturnType.GenericTypeArguments[0];
                invocation.ReturnValue = GetParamsErrorValueAsync((dynamic)Activator.CreateInstance(resultType), message);
                return;
            }
            //在被拦截的方法执行完毕后 继续执行
            invocation.Proceed();
            invocation.ReturnValue = GetReturnValueAsync((dynamic)invocation.ReturnValue);
        }


        private async Task<BaseResponse<T>> GetParamsErrorValueAsync<T>(BaseResponse<T> result, string message)
        {
            await Task.CompletedTask;
            return new BaseResponse<T> { Code = 400, Message = message };
        }


        private async Task<BaseResponse<T>> GetReturnValueAsync<T>(Task<BaseResponse<T>> task)
        {
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new BaseResponse<T> { Code = 200, Message = ex.Message };
            }
        }
    }
}
