using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Strong.Extensions.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Extensions.ServiceExtensions
{
    public static class HttpContextSetup
    {

        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }

    }
}
