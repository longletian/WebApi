using Serilog;
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
using Autofac;
using WebApi.WebApi.Api;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(
                option => option.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSingleton(new AppSetting(Configuration, Env));

            services.AddRedisService();

            services.AddSwaggUIService();

            services.AddMiniProfilerService();

            services.AddAutoMapper(typeof(AutoMapperHelper));

            services.AddEasyNetQService();

            //services.AddSignalR();

            services.AddCorsService();

            services.AddAuthenticationService();

            services.AddHttpClientService();

            services.AddResponseCachingService();

            //services.AddJwtService();

            services.AddResponseCompressionService();

        }

        #region 注入autofac

        /// <summary>
        /// 添加autofa服务 （注意：3.0写法 ）
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //新模块组件注册    
            builder.RegisterModule(new AutoFacModule());
        }

        #endregion

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //使用请求日志中间件
            app.UseSerilogRequestLogging();

            app.UseRoutingConfigure();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseSwaggUIConfigure();

            app.UseGrpcWeb();

            app.UseCors();

            app.UseMiniProfiler();

            app.UseLogMiddleware();

        }
    }
}
