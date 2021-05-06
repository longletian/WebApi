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

            #region �鿴�ӿڷ����ٶ�

            //services.AddEasyNetQService();

            //services.AddAuthenticationService();

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

            //��֤
            app.UseAuthentication();

            //��Ȩ
            app.UseAuthorization();

            // app.UseMiniProfiler();

            //ʹ��������־�м��
            app.UseSerilogRequestLogging();

            app.UseResponseCompression();
             
            app.UseResponseCaching();

            app.UseSwaggUIConfigure();

            //app.UseLogMiddleware();

            //app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                // ���ÿ���
                endpoints.MapGrpcService<GreeterService>()
                .EnableGrpcWeb()
                .RequireCors("AllowAll");

            endpoints.MapControllers();
                //��������Ӧ�ó������������
                //.RequireAuthorization();
            });
        }
    }
}
