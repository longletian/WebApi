using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Models;

namespace WebApi.Test.Data
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DataDbContext>();
                    db.Database.EnsureCreated();
                    try
                    {
                        Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            });
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