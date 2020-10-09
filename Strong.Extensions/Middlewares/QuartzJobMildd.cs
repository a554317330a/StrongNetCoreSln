using log4net;
using Microsoft.AspNetCore.Builder;
using SqlSugar;
using Strong.Common;
using Strong.IBussiness;
using Strong.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Extensions.Middlewares
{
    public static class QuartzJobMildd
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(QuartzJobMildd));
        public static void UseQuartzJobMildd(this IApplicationBuilder app, ITasksQzBussiness tasksQzServices, ISchedulerCenter schedulerCenter)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (Appsettings.app("AppSettings", "QuartzNetJob", "Enabled").ObjToBool())
                {

                    var allQzServices = tasksQzServices.QueryAsync().Result;
                    foreach (var item in allQzServices)
                    {
                        if (item.IsStart)
                        {
                            var ResuleModel = schedulerCenter.AddScheduleJobAsync(item).Result;
                            if (ResuleModel.success)
                            {
                                log.Info($"QuartzNetJob{item.Name}启动成功！");
                            }
                            else
                            {
                                log.Error($"QuartzNetJob{item.Name}启动失败！错误信息：{ResuleModel.msg}");
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                log.Fatal($"An error was reported when starting the job service.\n{e.Message}");
                throw;
            }
        }


    }
}
