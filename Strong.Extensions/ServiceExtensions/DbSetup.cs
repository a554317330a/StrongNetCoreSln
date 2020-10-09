using Microsoft.Extensions.DependencyInjection;
using Strong.Entities.Seed;
using System;

namespace Strong.Extensions.ServiceExtensions
{


    /// <summary>
    /// 生成数据库和种子数据，由于解耦。需要定义一个额外的数据库连接
    /// </summary>
    public static class DbSetup
    {
        public static void AddDbSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddScoped<DBSeed>();
            services.AddScoped<MyContext>();//这个Context是提供给创建数据的时候用的。
        }
    }
}
