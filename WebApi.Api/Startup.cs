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
using WebApi.Api.Middlewares;
using WebApi.Api.MiddlewareExtensions;

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
        /// ×¢²á·þÎñ
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(
                option => option.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSingleton(new AppSetting(Configuration, Env));

            services.AddFreeSqlService();

            services.AddRedisService();

            services.AddSwaggUIService();

            services.AddMiniProfilerService();

            services.AddAutoMapper(typeof(AutoMapperHelper));

            services.AddEasyNetQService();

            services.AddGrpc();

            services.AddCorsService();

            services.AddResponseCachingService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRoutingConfigure();

            app.UseSwaggUIConfigure();

            app.UseAuthorization();

            app.UseCors();

            app.UseMiniProfiler();

            app.UseLogMiddleware();

            app.UseResponseCaching();

        }
    }
}
