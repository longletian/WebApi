using FreeSql;
using System;
using System.IO;
using EasyNetQ;
using System.Linq;
using System.Text;
using WebApi.Models;
using System.Reflection;
using WebApi.Tools.Redis;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Profiling.Storage;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Common.Authorizations.JwtConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using FreeSql.Internal;
using Serilog;
using Microsoft.Extensions.Configuration;
using WebApi.Common.Freesql;

namespace WebApi.Api.ServiceExtensions
{
    public static class ServiceCollectionExtenioncs
    {

        /// <summary>
        ///  注入freesql
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddFreeSqlService(this IServiceCollection services, IConfiguration configuration)
        {
            IFreeSql freeSql = new FreeSqlBuilder()
                  //防止sql注入，开启lambda参数化功能 
                  .UseGenerateCommandParameterWithLambda(true)
                  .UseConnectionString(configuration)
                  //定义名称格式
                  .UseNameConvert(NameConvertType.ToLower)
                  .UseMonitorCommand(cmd =>
                  {
                      Log.Information(cmd.CommandText + ";");
                  })
                  .UseAutoSyncStructure(true) //自动同步实体结构到数据库
                  .Build(); //请务必定义成 Singleton 单例模式

            //增删改查触发
            freeSql.Aop.CurdAfter += (s, e) => {

            };

            services.AddSingleton(freeSql);
            services.AddFreeRepository();
            services.AddScoped<UnitOfWorkManager>();
            //全局过滤全部为false(是否删除)
            freeSql.GlobalFilter.Apply<IDeleteAduitEntity>("IsDeleted", a => a.IsDeleted == false);
            try
            {
                using var objPool = freeSql.Ado.MasterPool.Get();
            }
            catch (Exception e)
            {
                Log.Error(e + e.StackTrace + e.Message + e.InnerException);
                return;
            }
            //在运行时直接生成表结构
            try
            {
                freeSql.CodeFirst
                    .SeedData()
                    .SyncStructure(ReflexHelper.GetTypesByTableAttribute());
            }
            catch (Exception e)
            {
                Log.Logger.Error(e + e.StackTrace + e.Message + e.InnerException);
            }
        }

        /// <summary>
        /// 添加控制器数据验证
        /// </summary>
        /// <param name="services"></param>
        public static void AddControllService(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(option => option.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddFluentValidation(fv =>
                {
                    //是否同时支持两种验证方式
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    //自定义IValidator验证空接口
                    fv.RegisterValidatorsFromAssemblyContaining<Models.IValidator>();
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(x => x.Errors
                            .Select(p => p.ErrorMessage))
                        .ToList();

                    var result = new
                    {
                        Code = "405",
                        Message = "Validation errors",
                        Errors = errors
                    };
                    return new BadRequestObjectResult(result);
                };
            });
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
                // 引入Swashbuckle和FluentValidation
                c.AddFluentValidationRules();
            });
        }

        /// <summary>
        /// 注入miniprofile
        /// </summary>
        /// <param name="services"></param>
        public static void AddMiniProfilerService(this IServiceCollection services)
        {
            if (services == null)
            {
            }

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
                    .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
                    .WithExposedHeaders(tusdotnet.Helpers.CorsHelper.GetExposedHeaders());
            }));
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
                    ValidIssuer = jwtConfig.Issuer, //发行人
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience, //订阅人
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
                        new[] {"image/svg+xml"});
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
                string dbAble = AppSetting.GetConnStrings("DbAllow");
                if (!string.IsNullOrEmpty(dbAble))
                {
                    switch (dbAble)
                    {
                        case "MsSqlCon":
                            //x.UseKafka(AppSetting.GetConnStrings("MysqlCon"));
                            break;
                        case "PostgreSql":
                            x.UsePostgreSql(AppSetting.GetConnStrings("PostgreSqlCon"));
                            break;
                        default:
                            x.UseMySql(AppSetting.GetConnStrings("MysqlCon"));
                            break;
                    }
                }
                x.UseKafka(AppSetting.GetConnStrings("KafkaCon"));
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
            //将身份验证服务添加到DI和身份验证中间件到管道
            //注入身份认证服务
            services.AddAuthentication("Bearer")
                //  JWT 认证处理程序添加到DI中以供身份认证服务使用
                .AddJwtBearer("Bearer", options =>
                {
                    // 验证传入令牌以确保它来自受信任的颁发者
                    options.Authority = "https://localhost:5001";

                    // 验证token参数
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            //授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();

                    policy.RequireClaim("scope", "api1");
                });
            });
        }
    }
}