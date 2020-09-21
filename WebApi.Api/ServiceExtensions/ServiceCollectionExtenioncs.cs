using FreeSql;
using WebApi.Tools.Redis;
using WebApi.Common.AppSetting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;
using StackExchange.Profiling.Storage;
using EasyNetQ;

namespace WebApi.Api.ServiceExtensions
{
    public static class ServiceCollectionExtenioncs
    {
        /// <summary>
        /// 注入freesql
        /// </summary>
        /// <param name="services"></param>
        public static void AddFreeSqlService(this IServiceCollection services)
        {
            IFreeSql  freeSql  = new FreeSqlBuilder()
                 // 添加连接字符串和类型
                .UseConnectionString(DataType.MySql, AppSetting.GetConnStrings("MysqlCon"))
                // 自动同步实体结构到数据库
                .UseAutoSyncStructure(true)
                .Build();

            services.AddSingleton<IFreeSql>(freeSql);
            services.AddFreeRepository();
            services.AddScoped<UnitOfWorkManager>();
        }

        /// <summary>
        /// 注入csredis
        /// </summary>
        /// <param name="services"></param>
        public static void AddRedisService(this IServiceCollection services)
        {
            // 普遍模式
            var csredis = new CSRedis.CSRedisClient(AppSetting.GetConnStrings("RedisCon").ToString());
            // 初始化redisHelper
            RedisHelper.Initialization(csredis);

            // 注入Mvc分布式缓存CsRedisCache
            services.AddSingleton<IDistributedCache>(
                new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));

            services.AddSingleton<ICsRedisRepository, CsRedisRepository>();
        }

        public static void AddSwaggUIService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "xx",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        public static void AddMiniProfilerService(this IServiceCollection services)
        {
            if (services == null) {  }

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";

                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

                options.TrackConnectionOpenClose = true;

            });

        
        }
        public static void AddEasyNetQService(this IServiceCollection services)
        {
            services.AddSingleton(RabbitHutch.CreateBus(AppSetting.GetConnStrings("MqCon")));
        }

        public static  void  AddHttpClientService(this IServiceCollection services)
        {
            services.AddHttpClient();
        }

        public static void AddResponseCachingService(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }

        public static void AddCorsService(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));
        }
          

    }
}
