using Strong.Common;

namespace Strong.Repository.Base
{
    public static class BaseDBConfig
    {

        public static string ConnectionString = Appsettings.app(new string[] { "BaseConfig", "SqlConStr" });

    }
}
