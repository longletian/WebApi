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
                // IdentityResources �û����Ȩ��
                .AddInMemoryIdentityResources(InMemoryConfig.IdentityResources())
                // api����Ȩ��
                .AddInMemoryApiScopes(InMemoryConfig.ApiScopes)
                // �ͻ�������
                .AddInMemoryClients(InMemoryConfig.Clients())
                // �����û�
                .AddTestUsers(TestUsers.Users)

                //��չ��ÿ������ʱ��Ϊ����ǩ��������һ����ʱ��Կ
                .AddDeveloperSigningCredential();

            services.AddAuthentication()
            //���� Cookie �����������
            .AddCookie("Cookies")
            //.AddGoogle("Google", options =>
            //  {
            //      options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //      options.ClientId = "clientID";
            //      options.ClientSecret = "clientSecret";
            //  })
            //Զ�̲���
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

            //��֤
            //app.UseAuthentication();

            //��Ȩ
            app.UseAuthorization();

            //����app.UseAuthorization() ������������÷���Ҫ������app.UseRouting() and app.UseEndpoints(...).�м�

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
