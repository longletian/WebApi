using FreeSql;
using System;
using System.IO;
using EasyNetQ;
using MediatR;
using Serilog;
using System.Linq;
using System.Text;
using WebApi.Tools;
using WebApi.Models;
using WebApi.Common;
using FreeSql.Internal;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Common.Authorizations.JwtConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApi.Common.Authorizations.AuthorizationHandler;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;
using System.Net.Http;
using System.Configuration;
using WebApi.Tools.Mongodb;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Api.Filters;

namespace WebApi.Api
{
    public static class ServiceCollectionExtenioncs
    {
        /// <summary>
        /// 添加控制器数据验证
        /// </summary>
        /// <param name="services"></param>
        public static void AddControllService(this IServiceCollection services)
        {
            services.AddControllers((p) =>
            {
                //添加全局异常拦截
                p.Filters.Add(typeof(ExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddJsonOptions(option => option.JsonSerializerOptions.PropertyNamingPolicy = null)
            .AddFluentValidation(fv =>
            {
                //是否同时支持两种验证方式
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                //自定义IValidator验证空接口
                fv.RegisterValidatorsFromAssemblyContaining<Models.IValidator>();
            });

            //统一验证错误返回
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
            if (services == null) throw new ArgumentNullException(nameof(services));

            //// 普遍模式
            //var csredis = new CSRedis.CSRedisClient(AppSetting.GetConnStrings("CsRedis").ToString());
            //// 初始化redisHelper
            //RedisHelper.Initialization(csredis);

            //// 注入Mvc分布式缓存CsRedisCache
            //services.AddSingleton<IDistributedCache>(
            //    new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));

            ////单例模式
            //services.AddSingleton<ICsRedisRepository, CsRedisRepository>();

            //freeredis注册服务
            services.AddSingleton<ICacheBase, CacheBase>();
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
                    Title = "Web API",
                    Description = "ASP.NET Core Web API",
                    TermsOfService = new Uri("http://www.baidu.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "xx",
                        Email = string.Empty,
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // Jwt Bearer 认证，必须是 oauth2
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

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
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/mini-profiler-resources";

                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(10);

                options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.Left;

                options.PopupShowTimeWithChildren = true;

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

            // 实体映射
            services.Configure<JwtConfig>(AppSetting.GetSection("Audience"));

            JwtConfig jwtConfig = new JwtConfig();
            AppSetting.BindSection("Audience", jwtConfig);

            services.AddAuthentication(o =>
            {
                //添加JWT Scheme
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.Name,
                    //是否验证SecurityKey
                    ValidateIssuerSigningKey = true,
                    //加密key ascii编码
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.IssuerSigningKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer, //发行人
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience, //订阅人
                    // 是否验证token有效期
                    ValidateLifetime = true,
                    // 缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    //ClockSkew = TimeSpan.FromMinutes(jwtConfig.RefreshTokenExpiresMinutes),
                    ClockSkew = TimeSpan.FromSeconds(1),
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
                    },
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
        /// <param name="configuration"></param>
        public static void AddCapEvent(this IServiceCollection services, IConfiguration configuration)
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
                x.GetCapOptions(configuration);
                // 失败重试
                x.FailedRetryCount = 5;
            });
        }

        /// <summary>
        /// 注入IdServer4授权
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthenticationService(this IServiceCollection services)
        {
            //将身份验证服务添加到DI和身份验证中间件到管道
            //注入身份认证服务
            services.AddAuthentication(o =>
            {

                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //  JWT 认证处理程序添加到DI中以供身份认证服务使用
            .AddJwtBearer("Bearer", options =>
                {
                    // 验证传入令牌以确保它来自受信任的颁发者
                    //options.Authority = "https://localhost:5001";

                    TokenValidationParameters parameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = AppSetting.GetSection("JwtConfig:Issuer").Value.ToString(),//发行人
                        ValidAudience = AppSetting.GetSection("JwtConfig:Audience").Value.ToString(),//订阅人
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.GetSection("JwtConfig:IssuerSigningKey").Value.ToString()))
                    };


                    // 验证token参数
                    options.TokenValidationParameters = parameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hubs/messagehub")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        },

                        //在Token验证通过后调用
                        OnAuthenticationFailed = context =>
                        {
                            var jwtHandler = new JwtSecurityTokenHandler();
                            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                            if (!string.IsNullOrEmpty(token) && jwtHandler.CanReadToken(token))
                            {
                                var jwtToken = jwtHandler.ReadJwtToken(token);

                                if (jwtToken.Issuer != parameters.ValidIssuer)
                                {
                                    context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                                }

                                if (jwtToken.Audiences.FirstOrDefault() != parameters.ValidAudience)
                                {
                                    context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                                }
                            }

                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },

                        //未授权时调用失败
                        OnChallenge = context =>
                        {
                            if (context.Error != null)
                            {
                                string message = context.ErrorDescription;
                            }

                            context.Response.Headers.Add("Token-Error-Iss", "请授权");
                            return Task.CompletedTask;
                        },
                        //在Token验证通过后调用
                        //OnTokenValidated = context => { 
                        //}

                    };
                })
            .AddCookie();           
            //授权
            services.AddAuthorization(options =>
            {
                //配置授权
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();

                    policy.RequireClaim("scope", "api1");
                });

                //配置SignalR授权
                options.AddPolicy("SignalRAuthorizationPolicy", policy =>
                {
                    policy.Requirements.Add(new DomainRestrictedRequirement());
                });
            });
        }

        /// <summary>
        /// 实时 web 功能使服务器端代码可以立即将内容推送到客户端。
        /// </summary>
        /// <param name="services"></param>
        public static void AddSignalRService(this IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                //如果为，则在 true 集线器方法中引发异常时，
                //详细的异常消息将返回到客户端。 
                options.EnableDetailedErrors = true;
                //如果客户端未收到消息 (在此时间间隔内包含 keep-alive) ，
                //服务器将认为客户端已断开连接
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
                //如果服务器未在此时间间隔内发送消息，
                //则会自动发送 ping 消息，使连接保持打开状态。
                options.KeepAliveInterval = TimeSpan.FromMinutes(1);
            });
        }

        /// <summary>
        ///注入MediatR服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediatRService(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 注入rabbitmq
        /// </summary>
        public static void AddRabbitmqService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.Configure<RabbitmqOptions>(AppSetting.GetSection("RabbitMQ"));
            RabbitmqOptions options = new RabbitmqOptions();
            AppSetting.BindSection("RabbitMQ", options);
            if (options != null && options.Enabled)
            {
                services.AddSingleton<IRabbitmqConnection, RabbitmqConnection>();
            }
        }

        /// <summary>
        /// 注入eventstore
        /// </summary>
        public static void AddEventStoreService(this IServiceCollection services)
        {
            //var options = new EventStoreOption();
            //AppSetting.BindSection("EventStoresOptions", options);
            //services.AddSingleton<IMongoStore, MongoStore>();
            services.AddSingleton<IEventStores, EventStores>();
        }

        /// <summary>
        /// 注入mongodb
        /// </summary>
        /// <param name="services"></param>
        public static void AddMongodbService(this IServiceCollection services)
        {

            services.Configure<MongoStoreDatabaseSettings>(AppSetting.GetSection("MongoDbSettings"));
            services.AddSingleton<IMongoStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoStoreDatabaseSettings>>().Value);
            services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
        }

        /// <summary>
        /// 公共服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddCommonService(this IServiceCollection services)
        {
            services.Configure<JwtConfig>(AppSetting.GetSection("JwtConfig"));

            // 配置kestrel
            services.Configure<KestrelServerOptions>(AppSetting.GetSection("Kestrel"));

            // 延迟加载，注入（避免循环注入）
            services.AddTransient(typeof(Lazy<>), typeof(LazilyResolved<>));

            services.AddGrpc();

            //注册 gRPC 客户端
            services.AddGrpcClient<GreeterService>(o =>
            {
                o.Address = new Uri("https://localhost:5001");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                //handler.ClientCertificates.Add();
                return handler;
            });
            // 添加客户端在进行 gRPC 调用时将使用的 Interceptor 实例。
            //.AddInterceptor()
            //配置 gRPC 客户端的基础通道
            //.ConfigureChannel();

            services.Configure<FormOptions>(options =>
            {
                //超出设置范围会报InvalidDataException 异常信息
                //主要是限制缓冲形式中的文件的长度
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            //注入responserCache
            services.AddResponseCaching();
        }

        /// <summary>
        /// 添加内存缓存 
        /// </summary>
        /// <param name="services"></param>
        public static void AddCacheService(this IServiceCollection services)
        {
            // 分布式内存缓存，将项存储在内存中
            //services.AddDistributedMemoryCache();
        }
        /// <summary>
        ///  注入freesql
        /// </summary>
        /// <param name="services"></param>
        public static void AddFreeSqlService(this IServiceCollection services)
        {
            IFreeSql freeSql = new FreeSqlBuilder()
               //防止sql注入，开启lambda参数化功能 
               .UseGenerateCommandParameterWithLambda(true)
               .UseConnectionString()
               //定义名称格式
               .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
               .UseMonitorCommand(cmd =>
               {
                   Log.Information("Sql语句" + cmd.CommandText + ";");
               })
               .UseAutoSyncStructure(true) //自动同步实体结构到数据库
               .Build(); //请务必定义成 Singleton 单例模式

            //增删改查触发
            freeSql.Aop.CurdAfter += (s, e) =>
            {

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
    }
}