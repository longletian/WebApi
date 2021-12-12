using Serilog;
using Autofac;
using AutoMapper;
using WebApi.Common;
using WebApi.Common.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

            services.AddCommonService();

            services.AddFreeSqlService();

            services.AddSwaggUIService();

            services.AddControllService();
            
            services.AddAutoMapper(typeof(AutoMapperHelper));

            //services.AddCapEvent(Configuration);

            services.AddMongodbService();

            services.AddRedisService();

            services.AddCorsService();
            
            services.AddHttpClientService();

            services.AddRabbitmqService();

            services.AddResponseCompressionService();

            services.AddJwtService();

            services.AddEventStoreService();

            #region 查看接口访问速度

            //services.AddEasyNetQService();

            //services.AddAuthenticationService();

            services.AddMiniProfilerService();

            //services.AddMiniProfiler(options =>
            //{
            //    options.RouteBasePath = "/profile";
            //}).AddEntityFramework();
            #endregion
        }

        #region 注入autofac

        /// <summary>w
        /// 添加autofa服务 （注意：3.0写法 ）
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //新模块组件注册
            builder.RegisterModule(new AutoFacModule());
            builder.RegisterModule(new DependencyModule());
        }
        #endregion

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //使用请求日志中间件
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
            }
            app.UseSwaggUIConfigure(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("WebApi.Api.index.html"));

            app.UseCors();

            app.UseStaticFiles();

            app.UseRoutingConfigure();

            //认证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            app.UseResponseCompression();
             
            app.UseResponseCaching();

            //app.UseLogMiddleware();

            //app.UseGrpcWeb();

            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                // 配置跨域
                endpoints.MapGrpcService<GreeterService>()
                .EnableGrpcWeb()
                .RequireCors("AllowAll");

            endpoints.MapControllers();
                //禁用整个应用程序的匿名访问
                //.RequireAuthorization();
            });
        }
    }
}
