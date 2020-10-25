using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;

namespace WebApi.Common.AOP
{
    public class LogAop : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
