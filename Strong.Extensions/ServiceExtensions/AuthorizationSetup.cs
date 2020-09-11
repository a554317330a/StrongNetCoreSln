using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Extensions.ServiceExtensions
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 以下四种常见的授权方式。

            // 1、这个很简单，其他什么都不用做， 只需要在API层的controller上边，增加特性即可
            // [Authorize(Roles = "Admin,System")]

            //todo这里到时候继承一个父类控制器，然后父类控制器使用All

            //var allrole = new TB_ROLEBLL().Query().Select(o => o.RNAME).ToArray();
            var allrole = new List<string>();
            allrole.Add("Admin");
            // 2、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("All", policy => policy.RequireRole(allrole));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
