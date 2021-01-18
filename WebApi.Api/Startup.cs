using Serilog;
using Autofac;
using AutoMapper;
using WebApi.Common;
using WebApi.Common.AutoMapper;
using WebApi.Api.ServiceExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebApi.Api.ConfigureExtensions;
using WebApi.Api.MiddlewareExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Features;
using WebApi.Common.Authorizations.JwtConfig;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddFreeSqlService(Configuration);

            services.AddOptions<JwtConfig>(Configuration.GetSection("Audience").ToString());

            services.AddSwaggUIService();

            services.AddControllService();
            
            services.AddAutoMapper(typeof(AutoMapperHelper));

            services.AddCapEvent(Configuration);

            services.AddRedisService();

            services.AddCorsService();
            
            services.AddHttpClientService();

            services.AddRabbitmqService();

            services.Configure<FormOptions>(options =>
            {
                //超出设置范围会报InvalidDataException 异常信息
                //主要是限制缓冲形式中的文件的长度
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            services.AddResponseCachingService();
            
            services.AddResponseCompressionService();

            services.AddJwtService();

            services.AddEventStoreService();

            #region 查看接口访问速度

            //services.AddEasyNetQService();

            //services.AddAuthenticationService();

            // services.AddMiniProfilerService();

            // services.AddMiniProfiler(options =>
            // {
            //     options.RouteBasePath = "/profile";
            // }).AddEntityFramework();
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRoutingConfigure();

            app.UseCors();

            //认证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            // app.UseMiniProfiler();

            //使用请求日志中间件
            app.UseSerilogRequestLogging();

            app.UseResponseCompression();
             
            app.UseResponseCaching();

            app.UseSwaggUIConfigure();

            app.UseLogMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //禁用整个应用程序的匿名访问
                //.RequireAuthorization();
            });
        }
    }
}
