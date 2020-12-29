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
using WebApi.Repository;
using WebApi.Services;

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
        /// ע�����
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSetting(Configuration, Env));

            services.AddFreeSqlService(Configuration);

            services.AddSwaggUIService();

            services.AddControllService();
            
            services.AddAutoMapper(typeof(AutoMapperHelper));

            services.AddCapEvent(Configuration);

            services.AddRedisService();

            services.AddCorsService();
            
            services.AddHttpClientService();

            services.Configure<FormOptions>(options =>
            {
                //�������÷�Χ�ᱨInvalidDataException �쳣��Ϣ
                //��Ҫ�����ƻ�����ʽ�е��ļ��ĳ���
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            services.AddResponseCachingService();
            
            services.AddResponseCompressionService();

            // services.AddEasyNetQService();

            //services.AddSignalR();

            //services.AddAuthenticationService();

            //services.AddJwtService();

            #region �鿴�ӿڷ����ٶ�

            // services.AddMiniProfilerService();

            // services.AddMiniProfiler(options =>
            // {
            //     options.RouteBasePath = "/profile";
            // }).AddEntityFramework();
            #endregion

        }

        #region ע��autofac

        /// <summary>w
        /// ���autofa���� ��ע�⣺3.0д�� ��
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //��ģ�����ע��
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
            
            //ʹ��������־�м��
            app.UseSerilogRequestLogging();

            app.UseRoutingConfigure();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseSwaggUIConfigure();

            app.UseCors();

            app.UseLogMiddleware();

        }
    }
}
