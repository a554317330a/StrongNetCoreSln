using log4net;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Strong.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Strong.Extensions.ServiceExtensions
{
    public static class SqlsugarSetup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlsugarSetup));
        private static string _connectionString = Appsettings.app(new string[] { "BaseConfig", "SqlConStr" });
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddScoped<SqlSugar.ISqlSugarClient>(o =>
            {
                return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
                {
                    ConnectionString = _connectionString,//必填, 数据库连接字符串
                    DbType = DbType.SqlServer,//必填, 数据库类型
                    IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    InitKeyType = SqlSugar.InitKeyType.SystemTable,//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
                    AopEvents = new AopEvents
                    {
                        OnLogExecuting = (sql, p) =>
                        {
                            if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool())
                            {
                                Parallel.For(0, 1, e =>
                                {

                                    log.Info($"SQL：{ GetParas(p)}【SQL语句】：{sql}");
                                });
                            }
                        }
                    },
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsWithNoLockQuery = true,//查询锁
                        IsAutoRemoveDataCache = true//删除数据库缓存
                    }
                });
            });

        }
        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }


    }

}
