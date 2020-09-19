using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Extensions.ServiceExtensions
{
    public class HttpContextSetup
    {

        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }

    }
}
