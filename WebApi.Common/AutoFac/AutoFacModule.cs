using Autofac;
using System.Reflection;

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
            //builder.RegisterAssemblyTypes(GetAssemblyByName("WXL.Service")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();

            #endregion

            //builder.RegisterAssemblyTypes(GetAssemblyByName("WebApi.Repository")).Where(a => a.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces();

            //注册service服务
            builder.RegisterAssemblyTypes(GetAssemblyByName("WebApi.Services")).Where(a => a.Name.EndsWith("Services"))
                .AsImplementedInterfaces();

            //注册实体验证
            //builder.RegisterAssemblyTypes(GetAssemblyByName("WebApi.Models")).Where(a => a.Name.EndsWith("Dto"))
            //       .AsImplementedInterfaces();
        }

        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="AssemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(string AssemblyName)
        {
            return Assembly.LoadFrom(AssemblyName);
        }
    }
}
