using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace StrongAPI
{
    /// <summary>
    /// �������
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ��ڷ���
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// ����Web������
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>

        Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())//ʹ��AutoFac����
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>().UseKestrel(options =>
            {
                options.Limits.MaxConcurrentConnections = 100;
                options.Limits.MaxConcurrentUpgradedConnections = 100;
                options.Limits.MaxRequestBufferSize = 102400;
            })
            .ConfigureLogging((hostingContext, builder) =>
            {
                //���˵�ϵͳĬ�ϵ�һЩ��־
                builder.AddFilter("System", LogLevel.Error);
                builder.AddFilter("Microsoft", LogLevel.Error);
                //�������ļ�
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config");
                builder.AddLog4Net(path);
            })
            .UseUrls("https://*:8818");
        });

    }
}
