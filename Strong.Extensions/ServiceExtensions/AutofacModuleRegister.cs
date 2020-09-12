using Autofac;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

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

            try
            {

                // Service.dll 注入，有对应接口
                var assemblysServices = Assembly.LoadFile(servicesDllFile);
                builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();//指定已扫描程序集中的类型注册为提供所有其实现的接口。
                //Repository.dll 注入，有对应接口
                var assemblysRepository = Assembly.LoadFile(repositoryDllFile);
                builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            }
            catch (Exception ex)
            {
                throw new Exception("※※★※※ 如果你是第一次下载项目，请先对整个解决方案dotnet build（F6编译），然后再对api层 dotnet run（F5执行），\n因为解耦了，如果你是发布的模式，请检查bin文件夹是否存在Repository.dll和service.dll ※※★※※" + ex.Message + "\n" + ex.InnerException);
            }

        }
    }
}
