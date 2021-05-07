using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Common.Polly
{
    /// <summary>
    /// 异常弹性处理
    /// </summary>
    public class PollyHelper
    {
        private readonly ILogger<PollyHelper> _logger;
        public PollyHelper(ILogger<PollyHelper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 超时
        /// </summary>
        private AsyncTimeoutPolicy<HttpResponseMessage> TimeoutPolicy
        {
            get
            {
                return Policy.TimeoutAsync<HttpResponseMessage>(3, (context, span, task) =>
                {
                    _logger.LogInformation($"Policy: Timeout delegate fired after {span.Seconds} seconds");
                    return Task.CompletedTask;
                });
            }
        }

        /// <summary>
        /// 重试
        /// </summary>
        private AsyncRetryPolicy<HttpResponseMessage> RetryPolicy
        {
            get
            {
                HttpStatusCode[] retryStatus =
                {
                    HttpStatusCode.NotFound,
                    HttpStatusCode.ServiceUnavailable,
                    HttpStatusCode.RequestTimeout
                };
                return Policy
                    .HandleResult<HttpResponseMessage>(r => retryStatus.Contains(r.StatusCode))
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(new[]
                    {
                        // 表示重试3次，第一次1秒后重试，第二次2秒后重试，第三次4秒后重试
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(4)
                    }, (result, span, count, context) =>
                    {
                        _logger.LogInformation($"Policy: Retry delegate fired, attempt {count}");
                    });
            }
        }

        private AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakPolicy
        {
            get
            {
                HttpStatusCode[] retryStatus =
                {
                    HttpStatusCode.NotFound,
                    HttpStatusCode.ServiceUnavailable,
                    HttpStatusCode.RequestTimeout
                };
                return Policy.HandleResult<HttpResponseMessage>(r => retryStatus.Contains(r.StatusCode))
                    .CircuitBreakerAsync(3, TimeSpan.FromSeconds(5), (ex, ts) => {
                        _logger.LogInformation($"Policy: Timeout delegate fired after {ts.Seconds} seconds");
                    }, () => {

                    });
            }
        }

        /// <summary>
        /// 回退
        /// </summary>
        //private AsyncFallbackPolicy<HttpResponseMessage> FallbackPolicy
        //{
            //get
            //{
            //    HttpStatusCode[] retryStatus =
            //    {
            //        HttpStatusCode.NotFound,
            //        HttpStatusCode.ServiceUnavailable,
            //        HttpStatusCode.RequestTimeout
            //    };
            //    //return Policy.HandleResult<HttpResponseMessage>()
            //    //    .FallbackAsync(() =>
            //    //    {

            //    //    });
            //}
        //}

        /// <summary>
        /// 策略封装支持多个策略组合使用
        /// </summary>
        public AsyncPolicyWrap<HttpResponseMessage> PolicyStrategy =>
            Policy.WrapAsync(RetryPolicy, TimeoutPolicy, CircuitBreakPolicy);

        // 重试（Retry）
        public static void PollyRetry(Exception exception, int retryNum) {
            Policy.Handle<Exception>()
                .RetryAsync(3);

        }
        // 断路（Circuit-breaker）

        // 超时（Timeout）

        // 隔离（Bulkhead Isolation）

        // 回退（Fallback）

        // 缓存（Cache）

        // 策略包（Policy Wrap）
    }

    public class PolicyHandler : DelegatingHandler
    {
        private readonly PollyHelper _policies;

        public PolicyHandler(PollyHelper policies)
        {
            _policies = policies;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _policies.PolicyStrategy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }
    }
}

#region 操作
// 主动策略和被动策略
// 被动策略 
//重试:在实际应用场景中往往有些失败只是瞬时的，经过短暂的延时就可恢复，这种情况就可以采用重试策略；
//熔断:比如在调用接口发生异常时，当多次都返回异常，建议先熔断一段时间，即不再处理业务接口，直接报错；
//待熔断时间过了之后可以重新处理请求，即快速响应失败比让用户一直等待要合理；
//回退:如果失败之后怎么处理？即在发生故障的时候找一个替代逻辑进行处理， 比如返回指定的结果或是进行下一步操作；
// 主动策略
//超时:确保调用者永远不需要等待超过配置的超时时间，不然就会触发超时异常；主要就是为了提升用户体验
//舱壁隔离:即一个服务的故障不应该影响到整个系统(隔离)；
//通过控制资源消耗，避免一个故障导致级联服务也故障，最终影响整个系统；目的就是进行并发控制（限流），避免故障带来的大范围影响。
//缓存:将数据存入缓存中，后续的响应可以从缓存中获取; 目的就是为了提升性能
//策略包装:策略可以组合进行使用；目的就是为了方便各种策略组合进行业务故障处理；

/// <summary>
/// 重试测试
/// </summary>
//private static void RetryTest()
//{

//    #region 重试策略一
//    //var policy = Policy.Handle<DivideByZeroException>().Retry(5, (exception,index,context) => {

//    //    Console.WriteLine(exception.Message);
//    //    Console.WriteLine(index);
//    //    Console.WriteLine(context);
//    //});

//    //policy.Execute(() =>
//    //{
//    //    Console.WriteLine("开始处理业务------");
//    //    throw new DivideByZeroException();
//    //});

//    #endregion

//    #region 重试策略二
//    //Action<Exception, TimeSpan> onRetry = (exception, timespan) =>
//    //{
//    //    Console.WriteLine(exception.Message);
//    //    Console.WriteLine($"等待的时间{timespan}");
//    //};
//    ////返回值 x的 y 次幂
//    //Func<int, TimeSpan> sleepDurationProvider = retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount));
//    //var policy = Policy.Handle<DivideByZeroException>().WaitAndRetry(5, sleepDurationProvider, onRetry);

//    //policy.Execute(() =>
//    //{
//    //    Console.WriteLine("开始处理业务------");
//    //    throw new DivideByZeroException();
//    //});

//    #endregion


//    #region 熔断
//    //Action<Exception, TimeSpan> onBreak = (exception, timespan) =>
//    //{
//    //    Console.WriteLine(exception.Message);
//    //    Console.WriteLine($"开始触发时间{timespan}");
//    //};

//    //Action onReset = () =>
//    //{
//    //    Console.WriteLine("开始恢复");
//    //};

//    //var policy = Policy.Handle<DivideByZeroException>().CircuitBreaker(3, TimeSpan.FromSeconds(5), onBreak, onReset);

//    //for (int i = 0; i < 20; i++)
//    //{
//    //    try
//    //    {
//    //        policy.Execute(() =>
//    //        {
//    //            Console.WriteLine("开始处理业务------");
//    //            throw new DivideByZeroException();
//    //        });

//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        Console.WriteLine("爆出异常" + ex.Message);
//    //    }
//    //}
//    #endregion
//}

///// <summary>
///// 回退
///// </summary>
//private static void FallBackTest()
//{
//    //支持自定义返回信息
//    var policy = Policy<string>.Handle<DivideByZeroException>().Fallback("触发回退");
//    var returnValue = policy.Execute(() =>
//    {
//        Console.WriteLine("开始处理业务------");
//        throw new DivideByZeroException();
//    });

//    Console.WriteLine($"返回的结果{returnValue}");
//}

///// <summary>
///// 超时
///// </summary>
//private static void TimeOutTest()
//{

//    //Pessimistic 悲观超时,
//    //Optimistic  乐观超时
//    var policy = Policy.Timeout(5, Polly.Timeout.TimeoutStrategy.Optimistic, (context, time, task) =>
//    {
//        Console.WriteLine("业务超时了");
//        Console.WriteLine($"超时时间：{time.TotalSeconds}");
//        Console.WriteLine(task);
//    });

//    try
//    {
//        policy.Execute(() =>
//        {
//            Console.WriteLine("开始处理业务------");
//            Thread.Sleep(10000);
//            Console.WriteLine("业务处理完成");
//        });
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"爆出异常{ex.Message}");
//    }
//}

///// <summary>
///// 舱壁隔离
///// </summary>
//private static void BulkHeadTest()
//{

//    var policy = Policy.Bulkhead(5, (context) =>
//    {
//        Console.WriteLine($"触发隔离策略{context.OperationKey}");
//    });

//    for (int i = 0; i <= 20; i++)
//    {
//        Task.Run(() =>
//        {
//            try
//            {
//                policy.Execute(() =>
//                {
//                    Console.WriteLine("开始处理业务------");
//                    Thread.Sleep(1000);
//                });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"爆出异常{ex.Message}");
//            }
//        });
//    }
//}

///// <summary>
///// 缓存
///// </summary>
//private static void CacheTest()
//{
//    // 内存缓存基于MemoryCache
//    var memoryCache = new MemoryCache(new MemoryCacheOptions());
//    var memoryProvider = new MemoryCacheProvider(memoryCache);

//    //定义策略
//    var cachePolicy = Policy.Cache(memoryProvider, TimeSpan.FromSeconds(5));

//    for (int i = 0; i < 20; i++)
//    {
//        var cacheValue = cachePolicy.Execute((context) =>
//        {
//            Console.WriteLine($"开始执行{context.CorrelationId}");
//            return DateTime.Now;
//        }, new Context("cacheKey"));

//        Console.WriteLine($"第{i}次返回的值为{cacheValue}");
//        Thread.Sleep(1000);
//    }
//}

//private static void PolicyWarpTest()
//{
//    //定义回退策略
//    var policyFallback = Policy.Handle<DivideByZeroException>().Fallback(() =>
//    {
//        Console.WriteLine("触发回退事件处理");
//    });

//    // 定义超时策略
//    var polictTimeout = Policy.Timeout(5, Polly.Timeout.TimeoutStrategy.Pessimistic, (context, time, task) =>
//    {
//        Console.WriteLine("业务处理超时");
//        Console.WriteLine($"超时时间{time.TotalSeconds}");
//    });
//    var policyWarp = Policy.Wrap(polictTimeout, policyFallback);
//    try
//    {
//        policyWarp.Execute(() =>
//        {
//            Console.WriteLine("开始处理业务------");
//            Thread.Sleep(10000);
//            throw new DivideByZeroException();
//        });
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"爆出异常{ex.Message}");
//    }
//}
#endregion

