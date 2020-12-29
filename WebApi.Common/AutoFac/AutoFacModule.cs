﻿using Autofac;
using System.Reflection;
using Autofac.Extras.DynamicProxy;
using WebApi.Common.AOP;
using System;
using System.Collections.Generic;

namespace WebApi.Common.AutoFac
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoFacModule : Autofac.Module
    {
        /// <summary>
        /// 重写autofac管道load方法
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            #region 注入方式
            //1：类型注入
            //builder.RegisterType<>().As<>();
            //2：实例注入
            //var output = new StringWriter();
            //builder.RegisterInstance(output).As<TextWriter>();
            //3：对象注入
            //builder.Register(c => new ConfigReader("mysection")).As<IConfigReader>();
            //4：泛型注入
            //builder.RegisterGeneric(typeof(NHibernateRepository<>)) .As(typeof(IRepository<>)).InstancePerLifetimeScope();
            //5：程序集注入

            //注册拦截器
            //builder.RegisterType<ValidatorAop>();
            //对目标类型启动动态代理，并注入自定义拦截器拦截
            //builder.RegisterAssemblyTypes(GetAssemblyByName("WebApi.Models"))
            //        .Where(a => a.Name.EndsWith("Validator"))
            //        .AsImplementedInterfaces()
            //      .InstancePerDependency()
            //      .EnableInterfaceInterceptors().InterceptedBy(typeof(ValidatorAop));
            #endregion

            builder.RegisterAssemblyTypes(GetAssemblyByName("WebApi.Repository"))
                .Where(a => a.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerDependency();


            var cacheType = new List<Type>();
                builder.RegisterType<LogAop>();
                cacheType.Add(typeof(LogAop));

            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            builder.RegisterAssemblyTypes(GetAssemblyByName("WebApi.Services"))
                .Where(a => a.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerDependency()
                //引用Autofac.Extras.DynamicProxy;
                .EnableClassInterceptors()
                //允许将拦截器服务的列表分配给注册。
                .InterceptedBy(cacheType.ToArray());
        }

        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="AssemblyName"></param>
        /// <returns></returns>
        public Assembly GetAssemblyByName(string AssemblyName)
        {
            return Assembly.Load(AssemblyName);
        }
    }
}
