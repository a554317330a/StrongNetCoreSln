using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Strong.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Extensions.ServiceExtensions
{
    public static class JobSetup
    {
        public static void AddJobSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IJobFactory, JobFactory>();            
            services.AddTransient<UserJob>();//Job使用瞬时依赖注入
            services.AddSingleton<ISchedulerCenter, SchedulerCenterServer>();
        }
    }
}
