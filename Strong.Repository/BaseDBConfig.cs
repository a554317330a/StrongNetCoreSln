using Strong.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Repository
{
    public static class BaseDBConfig
    {

        public static string ConnectionString = Appsettings.app(new string[] { "BaseConfig", "SqlConStr" });

    }
}
