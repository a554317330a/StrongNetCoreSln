using Microsoft.Extensions.Configuration;

namespace Strong.Repository
{
    public class DbCollection
    {
        public static IConfiguration appSettingsJson = AppSettingsJson.GetAppSettings();
        public static string connectstr = appSettingsJson["JsonConfig:connectionStrings"];

      

        public static SqlSugarClient GetDb //注意当前方法的类不能是静态的 public static class这么写是错误的
        {
            get
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = connectstr, //必填, 数据库连接字符串
                    DbType = DbType.SqlServer, //必填, 数据库类型
                    IsAutoCloseConnection = true, //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    InitKeyType = InitKeyType.Attribute //默认SystemTable, 字段信息读取, 如：该属性是不是主键，是不是标识列等等信息
                });
              
            }
        }
    }
}
