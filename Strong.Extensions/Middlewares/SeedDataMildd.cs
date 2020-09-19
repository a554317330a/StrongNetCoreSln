using log4net;
using Microsoft.AspNetCore.Builder;
using SqlSugar;
using Strong.Common;
using Strong.Entities.Seed;
using System;

namespace Strong.Extensions.Middlewares
{
    public static class SeedDataMildd
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SeedDataMildd));
        public static void UseSeedDataMildd(this IApplicationBuilder app, MyContext myContext, string webRootPath)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                //生成数据库和种子数据
                if (Appsettings.app("AppSettings", "SeedDBInit").ObjToBool())
                {
                    DBSeed.SeedAsync(myContext, webRootPath).Wait();
                }
            }
            catch (Exception e)
            {
                log.Error($"Error occured seeding the Database.\n{e.Message}");

                log.Warn($"Warn occured seeding the Database.\n{e.Message}");

                throw;
            }
        }


    }
}
