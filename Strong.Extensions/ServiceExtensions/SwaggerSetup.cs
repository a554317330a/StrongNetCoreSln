
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Linq;
using static Strong.Extensions.ServiceExtensions.CustomApiVersion;

namespace Strong.Extensions.ServiceExtensions
{

    public static class SwaggerSetup
    {
        /// <summary>
        /// API名称
        /// </summary>
        private static string ApiName { get; set; } = "Strong.API";
        private static readonly ILog log = LogManager.GetLogger(typeof(SwaggerSetup));

        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            services.AddSwaggerGen(c =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        // {ApiName} 定义成全局变量，方便修改
                        Version = version,
                        Title = $"{ApiName} 接口文档——Netcore 3.0",
                        Description = $"{ApiName} HTTP API " + version,
                        Contact = new OpenApiContact { Name = ApiName, Email = "554317330@qq.com", Url = new Uri("http://www.iyuntu.com") },
                        License = new OpenApiLicense { Name = ApiName, Url = new Uri("http://www.iyuntu.com") }
                    });
                    c.OrderActionsBy(o => o.RelativePath);
                });
                #region XML文档
                try
                {
                    var xmlPath = Path.Combine(basePath, "Strong.API.xml");//这个就是刚刚配置的xml文件名
                    c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                    var xmlModelPath = Path.Combine(basePath, "Strong.Model.xml");//这个就是Model层的xml文件名
                    c.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception ex)
                {
                    log.Error("Strong.API.xml和Strong.Model.xml 丢失，请检查并拷贝。\n" + ex.Message);
                }
                #endregion


                //开启头部权限锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                c.OperationFilter<SecurityRequirementsOperationFilter>();//在header中添加token，传递到后台

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

            });
        }



    }

    /// <summary>
    /// 自定义版本
    /// </summary>
    public class CustomApiVersion
    {
        /// <summary>
        /// Api接口版本 自定义
        /// </summary>
        public enum ApiVersions
        {
            /// <summary>
            /// V1 版本
            /// </summary>
            V1 = 1,
            ///// <summary>
            ///// V2 版本
            ///// </summary>
            //V2 = 2,
        }
    }

}
