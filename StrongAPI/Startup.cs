using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Strong.API.AuthHelper;
using Strong.Common;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
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
            services.AddSingleton(new Appsettings(Configuration));
            //��ȡ�����ļ�
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = AppSecretConfig.Audience_Secret_String;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);


            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            


            #region ���ݿ�
            var sqlconstr = Configuration.GetConnectionString("SqlConStr");
            //services.AddDbContextPool<MyContext>(options => options.UseSqlServer(Configuration["ConfigOption:ConnectionString"]));
            #endregion

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

                #region �ĵ�

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

            

            #region ���ڶ�����������֤����

            // ���JwtBearer����
            services.AddAuthentication("Bearer").AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = signingKey,
                     ValidateIssuer = true,
                     ValidIssuer = audienceConfig["Issuer"],//������
                     ValidateAudience = true,
                     ValidAudience = audienceConfig["Audience"],//������
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.FromSeconds(30),// ����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ�� + ���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
                     RequireExpirationTime = true,
                 };
                 o.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         // ������ڣ����<�Ƿ����>��ӵ�������ͷ��Ϣ��
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             });

            #endregion


            #region ��Ȩ����
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("API", policy => policy.RequireRole("API").Build());
                options.AddPolicy("SystemOrClient", policy => policy.RequireRole("Admin", "Client"));
            });
            #endregion

            services.AddControllers();
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
    }
}
