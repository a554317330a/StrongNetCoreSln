using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Strong.API.Filter;
using Strong.Common;
using Strong.Common.Redis;
using Strong.Extensions.ServiceExtensions;
using Swashbuckle.AspNetCore.Filters;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace StrongAPI
{
    /// <summary>
    /// �������
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// API����
        /// </summary>
        public string ApiName { get; set; } = "Strong.API";

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// ����
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ����ע��services���񣨵�������EF��identity�ȣ��������У�ʹConfigure����ʹ����Щ����
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //��ȡ�����ļ�
            services.AddSingleton(new Appsettings(Configuration));
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();



            var symmetricKeyAsBase64 = AppSecretConfig.Audience_Secret_String;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

           
        


            #region JWT

            #region �ӿ��ĵ�Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    // {ApiName} �����ȫ�ֱ����������޸�
                    Version = "V1",
                    Title = $"{ApiName} �ӿ��ĵ�����Netcore 3.0",
                    Description = $"{ApiName} HTTP API V1",
                    Contact = new OpenApiContact { Name = ApiName, Email = "554317330@qq.com", Url = new Uri("http://www.iyuntu.com") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("http://www.iyuntu.com") }
                });
                c.OrderActionsBy(o => o.RelativePath);

                #region XML�ĵ�

                var xmlPath = Path.Combine(basePath, "Strong.API.xml");//������Ǹո����õ�xml�ļ���
                c.IncludeXmlComments(xmlPath, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�

                var xmlModelPath = Path.Combine(basePath, "Strong.Model.xml");//�������Model���xml�ļ���
                c.IncludeXmlComments(xmlModelPath);

                #endregion

                #region ����һ��,Swagger����JWT��֤��

                //����ͷ��Ȩ����
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });

                #endregion
            });
            #endregion

 

            #endregion




            //��Ȩ����
            services.AddAuthorizationSetup();
            // ���JwtBearer����
            services.AddAuthentication_JWTSetup();

             
            services.AddControllers(o =>
            {
                // ȫ���쳣����
                //o.Filters.Add(typeof(GlobalExceptionsFilter));
                // ȫ��·��ǰ׺��ͳһ�޸�·��
                o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
            })
            //ȫ������Json���л�����
            .AddNewtonsoftJson(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //��ʹ���շ���ʽ��key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss" ;
            });


            
        }

        /// <summary>
        /// ·�ɱ���ǰ׺����
        /// </summary>
        public static class RoutePrefix
        {
            /// <summary>
            /// ǰ׺��
            /// �������Ҫ���������գ���Ҫ�޸�
            /// ����һ��Ҫ�����е� api ǰͳһ�����ض�ǰ׺
            /// </summary>
            public const string Name = "[controller] /[action]";
        }
        /// <summary>
        /// ������������HTTP���������
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region �жϻ���

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();//ʹ��HTTP�ϸ�ȫ����
            }
            #endregion

            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName} V1");
                //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });
            #endregion

            //�û�����HTTPSͨ������HTTP�����ض���HTTPS�м����
            app.UseHttpsRedirection();

            app.UseRouting();

            //������֤
            app.UseAuthentication();
            //������Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            var servicesDllFile = Path.Combine(basePath, "Strong.Bussiness.dll");//��ȡע����Ŀ����·��
            var repositoryDllFile = Path.Combine(basePath, "Strong.Repository.dll");//��ȡע����Ŀ����·��

            //ע��Ҫͨ�����䴴�������

            //builder.RegisterType<BlogCacheAOP>();//����ֱ���滻����������
            //builder.RegisterType<BlogRedisCacheAOP>();//����ֱ���滻����������
            //builder.RegisterType<BlogLogAOP>();//��������ע��ڶ���

            try
            {

                // Service.dll ע�룬�ж�Ӧ�ӿ�
                var assemblysServices = Assembly.LoadFile(servicesDllFile);
                builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();//ָ����ɨ������е�����ע��Ϊ�ṩ������ʵ�ֵĽӿڡ�
                //Repository.dll ע�룬�ж�Ӧ�ӿ�
                var assemblysRepository = Assembly.LoadFile(repositoryDllFile);
                builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

                // AOP ���أ������Ҫ��ָ���Ĺ��ܣ�ֻ��Ҫ�� appsettigns.json ��Ӧ��Ӧ true ���С�
                var cacheType = new List<Type>();
                //if (Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
                //{
                //    cacheType.Add(typeof(BlogRedisCacheAOP));
                //}
                //if (Appsettings.app(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool())
                //{
                //    cacheType.Add(typeof(BlogCacheAOP));
                //}
                //if (Appsettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
                //{
                //    cacheType.Add(typeof(BlogLogAOP));
                //}



            }
            catch (Exception ex)
            {
                throw new Exception("��������� ������ǵ�һ��������Ŀ�����ȶ������������dotnet build��F6���룩��Ȼ���ٶ�api�� dotnet run��F5ִ�У���\n��Ϊ�����ˣ�������Ƿ�����ģʽ������bin�ļ����Ƿ����Repository.dll��service.dll ���������" + ex.Message + "\n" + ex.InnerException);
            }

        }


    }
}
