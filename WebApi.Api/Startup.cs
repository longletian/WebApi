using Serilog;
using Autofac;
using AutoMapper;
using WebApi.Common.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebApi.Api.ConfigureExtensions;
using WebApi.Api.ServiceExtensions;
using WebApi.Api.MiddlewareExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Common.AutoFac;
using WebApi.Common;
using Microsoft.AspNetCore.Http.Features;

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
            services.AddSingleton(new AppSetting(Configuration, Env));

            services.AddSwaggUIService();

            services.AddControllService();

            services.AddDataDbContext();
            
            services.AddAutoMapper(typeof(AutoMapperHelper));
            
            // services.AddRedisService();

            services.AddCorsService();
            
            services.AddHttpClientService();

            services.Configure<FormOptions>(options =>
            {
                //超出设置范围会报InvalidDataException 异常信息
                //主要是限制缓冲形式中的文件的长度
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            services.AddResponseCachingService();
            
            services.AddResponseCompressionService();

            // services.AddEasyNetQService();
            
            // services.AddMiniProfilerService();

            //services.AddSignalR();
            
            //services.AddAuthenticationService();

            //services.AddJwtService();

            // services.AddMiniProfiler(options =>
            // {
            //     options.RouteBasePath = "/profile";
            // }).AddEntityFramework();
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

            // app.UseMiniProfiler();
            
            //使用请求日志中间件
            app.UseSerilogRequestLogging();

            app.UseRoutingConfigure();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseSwaggUIConfigure();

            app.UseCors();

            //app.UseCap();

            app.UseLogMiddleware();

        }
    }
}
