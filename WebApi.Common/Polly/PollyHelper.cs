using Polly;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Common.Polly
{
    /// <summary>
    /// 异常弹性处理
    /// </summary>
    public class PollyHelper
    {
        // 重试（Retry）
        public void PollyRetry(Exception exception, int retryNum) {
            Policy.Handle<Exception>()
                .RetryAsync(with => with.RetryCount(3));

        }
        // 断路（Circuit-breaker）

        // 超时（Timeout）

        // 隔离（Bulkhead Isolation）

        // 回退（Fallback）

        // 缓存（Cache）

        // 策略包（Policy Wrap）
    }
}
