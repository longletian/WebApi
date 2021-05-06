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
        private AsyncFallbackPolicy<HttpResponseMessage> FallbackPolicy
        {
            get
            {
                HttpStatusCode[] retryStatus =
                {
                    HttpStatusCode.NotFound,
                    HttpStatusCode.ServiceUnavailable,
                    HttpStatusCode.RequestTimeout
                };
                //return Policy.HandleResult<HttpResponseMessage>()
                //    .FallbackAsync(() =>
                //    {

                //    });
            }
        }


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
