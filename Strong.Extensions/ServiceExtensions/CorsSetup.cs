using Microsoft.Extensions.DependencyInjection;
using Strong.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Extensions.ServiceExtensions
{
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(c =>
            {
                if (!Appsettings.app(new string[] { "Startup", "Cors", "EnableAllIPs" }).ObjToBool())
                {
                    c.AddPolicy(Appsettings.app(new string[] { "Startup", "Cors", "PolicyName" }),

                        policy =>
                        {

                            policy
                            .WithOrigins(Appsettings.app(new string[] { "Startup", "Cors", "IPs" }).Split(','))
                            .AllowAnyHeader()//Ensures that the policy allows any header.
                            .AllowAnyMethod();
                        });
                }
                else
                {
                    //允许任意跨域请求
                    c.AddPolicy(Appsettings.app(new string[] { "Startup", "Cors", "PolicyName" }),
                        policy =>
                        {
                            policy
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
                }

            });
        }

    }
}
