using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Common.AutoFac;
using WebApi.Models;

namespace WebApi.Test.Data
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //配置数据库内存测试
            builder.ConfigureServices(services =>
            {

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var freeSql = scopedServices.GetRequiredService<IFreeSql>();
                    freeSql.CodeFirst.IsAutoSyncStructure=true;
                    try
                    {
                        Utilities.InitializeDbForTests(freeSql);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
            
            //注入autofac
            var service = A.Fake<AutoFacModule>();
            
            void ConfigureTestContainer(ContainerBuilder builder) {
                builder.RegisterInstance(service);
            }
            
            builder.ConfigureTestContainer<ContainerBuilder>(ConfigureTestContainer);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new CustomServiceProviderFactory());
            return base.CreateHost(builder);
        }
    }

    public class CustomServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private AutofacServiceProviderFactory _wrapped;
        private IServiceCollection _services;

        public CustomServiceProviderFactory()
        {
            _wrapped = new AutofacServiceProviderFactory();
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            _services = services;

            return _wrapped.CreateBuilder(services);
        }

        /// <summary>
        /// 为了解决autofac6版本的问题
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            var sp = _services.BuildServiceProvider();
#pragma warning disable CS0612 // Type or member is obsolete
            var filters = sp.GetRequiredService<IEnumerable<IStartupConfigureContainerFilter<ContainerBuilder>>>();
#pragma warning restore CS0612 // Type or member is obsolete

            foreach (var filter in filters)
            {
                filter.ConfigureContainer(b => { })(containerBuilder);
            }

            return _wrapped.CreateServiceProvider(containerBuilder);
        }
    }
}