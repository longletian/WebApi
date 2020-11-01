using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using WebApi.IDS4.Config;

namespace IdServer4.WebApi.IDS4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
 

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                // IdentityResources 用户相关权限
                .AddInMemoryIdentityResources(InMemoryConfig.IdentityResources())
                // api访问权限
                .AddInMemoryApiScopes(InMemoryConfig.ApiScopes)
                // 客户端配置
                .AddInMemoryClients(InMemoryConfig.Clients())
                // 测试用户
                .AddTestUsers(TestUsers.Users)

                //扩展在每次启动时，为令牌签名创建了一个临时密钥
                .AddDeveloperSigningCredential();

            services.AddAuthentication()
            //覆盖 Cookie 处理程序配置
            .AddCookie("Cookies")
            //.AddGoogle("Google", options =>
            //  {
            //      options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //      options.ClientId = "clientID";
            //      options.ClientSecret = "clientSecret";
            //  })
            //远程测试
            .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.SaveTokens = true;

                options.Authority = "https://demo.identityserver.io/";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            //认证
            //app.UseAuthentication();

            //授权
            app.UseAuthorization();

            //调用app.UseAuthorization() ，并且这个调用方法要出现在app.UseRouting() and app.UseEndpoints(...).中间

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
