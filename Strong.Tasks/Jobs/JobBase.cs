using Quartz;
using Strong.Common;
using Strong.Entities.DBModel;
using Strong.IBussiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Strong.Tasks.Jobs
{
   public class JobBase
    {
        //todo 后面需要记录到执行记录表

        /// <summary>
        /// 执行指定任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public async Task<int> ExecuteJob(IJobExecutionContext context,ITB_JobLogBussiness _JobLogBussiness, Func<Task> func)
        {
            var model = new TB_JobLog();
            try
            {
              

                model.L_JobId = context.Trigger.Key.Name.ObjToInt(); 
                model.L_StartTime = DateTime.Now;
                //记录Job时间
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await func();//执行任务
                stopwatch.Stop();
                model.L_Result = true;
                model.Message = "执行成功";
                model.L_SpendTime = stopwatch.Elapsed.TotalMilliseconds.ObjToDecimal();
              
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                e2.RefireImmediately = true;
                model.L_JobId = context.Trigger.Key.Name.ObjToInt(); 
                model.L_StartTime = DateTime.Now;
                model.L_Result = false;
                model.Message = ex.Message;
               

            }
           return await _JobLogBussiness.AddAsync(model);

        }

    }
}
