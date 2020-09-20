using Autofac;
using Autofac.Extras.DynamicProxy;
using log4net;
using SqlSugar;
using Strong.Common;
using Strong.Extensions.Account;
using Strong.Extensions.AOP;
using Strong.IRepository.Base;
using Strong.Repository.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
 

namespace Strong.Extensions.ServiceExtensions
{
    public class AutofacModuleRegister : Autofac.Module
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(AutofacModuleRegister));
        protected override void Load(ContainerBuilder builder)
        {

            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            var servicesDllFile = Path.Combine(basePath, "Strong.Bussiness.dll");//获取注入项目绝对路径
            var repositoryDllFile = Path.Combine(basePath, "Strong.Repository.dll");//获取注入项目绝对路径


            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            var cacheType = new List<Type>();
            if (Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<RedisCacheAOP>();
                cacheType.Add(typeof(RedisCacheAOP));
            }
            //if (Appsettings.app(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
            //{
            //    builder.RegisterType<BlogTranAOP>();
            //    cacheType.Add(typeof(BlogTranAOP));
            //}
             builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();//注册仓储
            try
            {
                // 获取 Service.dll 程序集服务，并注册
                var assemblysServices = Assembly.LoadFrom(servicesDllFile);
                builder.RegisterAssemblyTypes(assemblysServices)
                          .AsImplementedInterfaces()
                          .InstancePerDependency()
                          .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                          .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。

                // 获取 Repository.dll 程序集服务，并注册
                var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
                builder.RegisterAssemblyTypes(assemblysRepository)
                       .AsImplementedInterfaces()
                       .InstancePerDependency();

                builder.RegisterType<AspNetUser>().As<IUser>();
          

            }
            catch (Exception ex)
            {
                throw new Exception("※※★※※ 如果你是第一次下载项目，请先对整个解决方案dotnet build（F6编译），然后再对api层 dotnet run（F5执行），\n因为解耦了，如果你是发布的模式，请检查bin文件夹是否存在Repository.dll和service.dll ※※★※※" + ex.Message + "\n" + ex.InnerException);
            }

        }
    }
}
