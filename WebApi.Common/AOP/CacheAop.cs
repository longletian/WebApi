using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Common
{
    /// <summary>
    /// 缓存aop
    /// </summary>
    public class CacheAop : IInterceptor
    {
        private readonly IMemoryCache memoryCache;
        public CacheAop(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }

        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
