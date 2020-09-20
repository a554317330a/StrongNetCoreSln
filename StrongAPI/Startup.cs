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
using Strong.Model.Common;
using System.Text;


namespace Strong.API
{
    /// <summary>
    /// 入口配置
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        private IServiceCollection _services;
        
        /// <summary>
        /// API名称
        /// </summary>
        public string ApiName { get; set; } = "Strong.API";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }



        /// <summary>
        /// 用于注册services服务（第三方，EF，identity等）到容器中，使Configure可以使用这些服务
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //读取配置文件
            services.AddSingleton(new Appsettings(Configuration)); 

            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();//单例Redis缓存,必须单例的哦，不然会爆--https://www.cnblogs.com/JulianHuang/p/11541658.html

            services.Configure<JsonConfig>(opts => Configuration.GetSection("JsonConfig").Bind(opts));


            //种子数据
            services.AddDbSetup();
            //跨域
            services.AddCorsSetup();
            //添加Swagger
            services.AddSwaggerSetup();

            services.AddHttpContextSetup();
            //授权对象
            services.AddAuthorizationSetup();
            // 添加JwtBearer服务
            services.AddAuthentication_JWTSetup();

            //控制器过滤
            services.AddControllers(o =>
            {
                //全局控制器方法过滤
                o.Filters.Add(typeof(ActionFilter));
                // 全局异常过滤
                //o.Filters.Add(new ExceptionFilter());
                // 全局路由前缀，统一修改路由
                o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
            })
            //全局配置Json序列化处理
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            _services = services;


        }

        /// <summary>
        /// 路由变量前缀配置
        /// </summary>
        public static class RoutePrefix
        {
            /// <summary>
            /// 前缀名
            /// 如果不需要，尽量留空，不要修改
            /// 除非一定要在所有的 api 前统一加上特定前缀
            /// </summary>
            public const string Name = "[controller] /[action]";
        }
        /// <summary>
        /// 用于配置整个HTTP请求的流程
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, MyContext myContext, IWebHostEnvironment env)
        {


            // 查看注入的所有服务
            app.UseAllServicesMildd(_services);
            #region 判断环境

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();//使用HTTP严格安全传输
            }
            #endregion

            // 封装Swagger
            app.UseSwaggerMildd();

            // CORS跨域
            app.UseCors(Appsettings.app(new string[] { "Startup", "Cors", "PolicyName" }));

            //用户构建HTTPS通道（将HTTP请求重定向到HTTPS中间件）
            //app.UseHttpsRedirection();

            // 使用静态文件
            app.UseStaticFiles();
            // 使用cookie
            //app.UseCookiePolicy();
            // 返回错误码
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();

            //开启认证
            app.UseAuthentication();
            //开启授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSeedDataMildd(myContext, Env.WebRootPath);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }


    }
}
