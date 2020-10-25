using AutoMapper;
using WebApi.Common.AppSetting;
using WebApi.Common.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Api.ConfigureExtensions;
using WebApi.Api.ServiceExtensions;
using WebApi.Api.MiddlewareExtensions;
using Serilog;
using Autofac;
using WebApi.Common.AutoFac;
using System;

namespace WebApi.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment Env { get; }
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(
                option => option.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSingleton(new AppSetting(Configuration, Env));

            services.AddRedisService();

            services.AddSwaggUIService();

            services.AddMiniProfilerService();

            services.AddAutoMapper(typeof(AutoMapperHelper));

            services.AddEasyNetQService();

            services.AddGrpc();

            services.AddSignalR();

            services.AddCorsService();

            services.AddHttpClientService();

            services.AddResponseCachingService();

            services.AddJwtService();

            services.AddResponseCompressionService();

            return services.AddAutoFac();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //使用请求日志中间件
            app.UseSerilogRequestLogging();

            app.UseAuthentication();

            app.UseRoutingConfigure();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseSwaggUIConfigure();

            app.UseAuthorization();

            app.UseGrpcWeb();

            app.UseCors();

            app.UseMiniProfiler();

            app.UseLogMiddleware();

        }
    }
}
