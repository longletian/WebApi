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
using Microsoft.AspNetCore.Http;
using WebApi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebApi.Common.Authorizations.JwtConfig;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using WebApi.Common.AutoFac;
using DotNetCore.CAP.Dashboard.NodeDiscovery;

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
            IFreeSql freeSql = new FreeSqlBuilder()
                // 添加连接字符串和类型
                .UseConnectionString(DataType.MySql, AppSetting.GetConnStrings("MysqlCon"))
                // 自动同步实体结构到数据库
                // 注意：不要在生产环境随意开启
                .UseAutoSyncStructure(true)
                // 开启延时加载
                .UseLazyLoading(true)
                .Build();

            services.AddSingleton(freeSql);
            //services.AddFreeRepository();
            //services.AddScoped<UnitOfWorkManager>();
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

        /// <summary>
        /// 注入swaggerui
        /// </summary>
        /// <param name="services"></param>
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

        /// <summary>
        /// 注入miniprofile
        /// </summary>
        /// <param name="services"></param>
        public static void AddMiniProfilerService(this IServiceCollection services)
        {
            if (services == null) { }

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";

                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

                options.TrackConnectionOpenClose = true;

            });
        }

        /// <summary>
        /// 注入EasyNetQ
        /// </summary>
        /// <param name="services"></param>
        public static void AddEasyNetQService(this IServiceCollection services)
        {
            services.AddSingleton(RabbitHutch.CreateBus(AppSetting.GetConnStrings("MqCon")));
        }

        /// <summary>
        /// 注入httpclient
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpClientService(this IServiceCollection services)
        {

            services.AddHttpClient();
            services.AddHttpClient("github", c =>
            {
                // 设置基础地址
                c.BaseAddress = new Uri("");
                // 设置默认请求头
                c.DefaultRequestHeaders.Add("Accept", "");
            });
            // 注入IHttpClientFactory _httpClientFactory
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// 注入responserCache
        /// </summary>
        /// <param name="services"></param>
        public static void AddResponseCachingService(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }

        /// <summary>
        /// 注入跨域
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsService(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       // 允许浏览器应用进行跨域 gRPC-Web 调用
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));
        }

        /// <summary>
        /// 注入dbcontext
        /// </summary>
        /// <param name="services"></param>
        public static void AddDataDbContext(this IServiceCollection services)
        {
            services.AddDbContext<DataDbContext>(options =>
            {
                switch (AppSetting.GetConnStrings("DbAllow"))
                {
                    case "MSSQL":
                        options.UseSqlServer(AppSetting.GetConnStrings("MSSQLCon"));
                        break;

                    default:
                        options.UseMySQL(AppSetting.GetConnStrings("MysqlCon"));
                        break;
                }
            });
        }

        /// <summary>
        /// 注入jwt
        /// </summary>
        /// <param name="services"></param>
        public static void AddJwtService(this IServiceCollection services)
        {

            if (services == null) throw new ArgumentNullException(nameof(services));

            services.Configure<JwtConfig>(AppSetting.GetSection("Audience"));

            JwtConfig jwtConfig = new JwtConfig();
            AppSetting.BindSection("Audience", jwtConfig);
            var keyByteArray = Encoding.ASCII.GetBytes(jwtConfig.IssuerSigningKey);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            services.AddAuthentication(o =>
            {
                //添加JWT Scheme
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.Name,
                    //是否验证SecurityKey
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,//发行人
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,//订阅人
                    // 验证失效时间
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(jwtConfig.RefreshTokenExpiresMinutes),
                    RequireExpirationTime = true,
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        var jwtToken = (new JwtSecurityTokenHandler()).ReadJwtToken(token);

                        if (jwtToken.Issuer != jwtConfig.Issuer)
                        {
                            context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                        }

                        if (jwtToken.Audiences.FirstOrDefault() != jwtConfig.Audience)
                        {
                            context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                        }

                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };

            });
        }

        /// <summary>
        /// 注入响应压缩
        /// </summary>
        public static void AddResponseCompressionService(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "image/svg+xml" });
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest;
            });
        }

        /// <summary>
        /// 注入cap
        /// </summary>
        /// <param name="services"></param>
        public static void AddCapEvent(this IServiceCollection services)
        {
            #region cap简介
            // CAP 是一个在分布式系统中实现事件总线及最终一致性（分布式事务）
            // 具有轻量级，高性能，易使用等特点。
            // 具有 Event Bus 的所有功能，并且CAP提供了更加简化的方式来处理EventBus中的发布 / 订阅
            // 具有消息持久化的功能
            // 实现了分布式事务中的最终一致性，
            #endregion

            services.AddCap(x =>
            {
                x.UseDashboard();
                //x.UseMySql(AppSetting.GetConnStrings("MysqlCon"));
                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = AppSetting.GetSection("MQ:Host").ToString();
                    cfg.VirtualHost = AppSetting.GetSection("MQ:VirtualHost").ToString();
                    cfg.Port = Convert.ToInt32(AppSetting.GetSection("MQ:Port"));
                    cfg.UserName = AppSetting.GetSection("MQ:UserName").ToString();
                    cfg.Password = AppSetting.GetSection("MQ:Password").ToString();
                });
                x.FailedRetryInterval = 5;
                x.FailedRetryCount = 2;

                #region 注入Consul
                //x.UseDiscovery(d =>
                //{
                //    d.DiscoveryServerHostName = "localhost";
                //    d.DiscoveryServerPort = 8500;
                //    d.CurrentNodeHostName = "localhost";
                //    d.CurrentNodePort = 5800;
                //    d.NodeId = 1;
                //    d.NodeName = "CAP No.1 Node";

                //});
                #endregion

            });
        }


        public static void AddAuthenticationService(this IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   options.Authority = "https://localhost:5000";

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false
                   };
               });
        }

      
    }
}
