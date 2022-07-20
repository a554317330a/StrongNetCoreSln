using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Strong.API.Filter;
using Strong.Common;
using Strong.Common.Redis;
using Strong.Entities.Seed;
using Strong.Extensions.Middlewares;
using Strong.Extensions.ServiceExtensions;
using Strong.IBussiness;
using Strong.Model.Common;
using Strong.Tasks;
using System.Text;


namespace Strong.API
{
    /// <summary>
    /// �������
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        private IServiceCollection _services;
        
        /// <summary>
        /// API����
        /// </summary>
        public string ApiName { get; set; } = "Strong.API";

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }



        /// <summary>
        /// ����ע��services���񣨵�������EF��identity�ȣ��������У�ʹConfigure����ʹ����Щ����
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   

            
               //测试
            //��ȡ�����ļ�
            services.AddSingleton(new Appsettings(Configuration)); 

            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();//����Redis����,���뵥����Ŷ����Ȼ�ᱬ--https://www.cnblogs.com/JulianHuang/p/11541658.html

            services.Configure<JsonConfig>(opts => Configuration.GetSection("JsonConfig").Bind(opts));
            //�ִ�
            services.AddSqlsugarSetup();
            //��������
            services.AddDbSetup();
            //����
            services.AddCorsSetup();
            //����Swagger
            services.AddSwaggerSetup();
            //�������
            services.AddJobSetup();
            services.AddHttpContextSetup();
            //��Ȩ����
            services.AddAuthorizationSetup();
            // ����JwtBearer����
            services.AddAuthentication_JWTSetup();

            //����������
            services.AddControllers(o =>
            {
                //ȫ�ֿ�������������
                o.Filters.Add(typeof(ActionFilter));
               // ȫ���쳣����
                o.Filters.Add(typeof(ExceptionFilter));
                o.Filters.Add(typeof(ResultFilter));
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
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            _services = services;


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
            public const string Name = "[controller]/[action]";
        }
        /// <summary>
        /// ������������HTTP���������
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, MyContext myContext, ITasksQzBussiness tasksQzServices, ISchedulerCenter schedulerCenter, IHostApplicationLifetime lifetime, IWebHostEnvironment env)
        {


            // �鿴ע������з���
            app.UseAllServicesMildd(_services);
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

            // ��װSwagger
            app.UseSwaggerMildd();

            // CORS����
            app.UseCors(Appsettings.app(new string[] { "Startup", "Cors", "PolicyName" }));

            //�û�����HTTPSͨ������HTTP�����ض���HTTPS�м����
            //app.UseHttpsRedirection();

            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            // ʹ��cookie
            //app.UseCookiePolicy();
            // ���ش�����
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();

            //������֤
            app.UseAuthentication();
            //������Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //��������
            app.UseSeedDataMildd(myContext, Env.WebRootPath);
            // ����QuartzNetJob���ȷ���
            app.UseQuartzJobMildd(tasksQzServices, schedulerCenter);
            //����ע��
            //app.UseConsulMildd(Configuration, lifetime);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }


    }
}
