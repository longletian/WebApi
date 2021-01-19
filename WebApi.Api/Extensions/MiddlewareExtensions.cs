using Microsoft.AspNetCore.Builder;
using WebApi.Api.Common.Middlewares;

namespace WebApi.Api
{

    /// <summary>
    /// 扩展中间件
    /// </summary>
    public static class MiddlewareExtensions
    {

        /// <summary>
        ///异常中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void UseLogMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogMiddleware>();
        }
    }
}
