using Microsoft.AspNetCore.Builder;
using WebApi.Tools.SignalR;

namespace WebApi.Api.ConfigureExtensions
{
    public static class ConfigureExtensions
    {
       /// <summary>
       /// swagger-ui
       /// </summary>
       /// <param name="app"></param>
        public static void UseSwaggUIConfigure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        /// <summary>
        /// 路由配置
        /// </summary>
        /// <param name="app"></param>
        public static void UseRoutingConfigure(this IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 使用实时通讯
        /// </summary>
        /// <param name="app"></param>
        public static void UseSignalConfigure(this IApplicationBuilder app)
        {
            app.UseSignalR(routes =>
            {
                routes.MapHub<MessageHub>("/messagehub");
            });
        }
    }
}
