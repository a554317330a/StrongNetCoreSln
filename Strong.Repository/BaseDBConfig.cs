using Strong.Common;

namespace Strong.Repository
{
    public static class BaseDBConfig
    {

        public static string ConnectionString = Appsettings.app(new string[] { "BaseConfig", "SqlConStr" });

    }
}
