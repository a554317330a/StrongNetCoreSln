using Quartz;
using SqlSugar;
using Strong.Common.Helper;
using Strong.IBussiness;
using Strong.Tasks.Jobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Strong.Tasks  
{
    public class UserJob : JobBase, IJob
    {
        private readonly ITB_UserBussiness _userbussiness;
        private readonly ITasksQzBussiness _tasksQzServices;
        private readonly ITB_JobLogBussiness _JobLogBussiness;

        public UserJob(ITB_UserBussiness _userBussiness,  ITB_JobLogBussiness jobLogBussiness,ITasksQzBussiness tasksQzServices)
        {
            _JobLogBussiness = jobLogBussiness;
            _userbussiness = _userBussiness;
            _tasksQzServices = tasksQzServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {

 
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;
            var executeLog = await ExecuteJob(context,_JobLogBussiness, async () => await Run(context, jobId.ObjToInt()));
            JobDataMap data = context.JobDetail.JobDataMap;
          

        }
        public async Task Run(IJobExecutionContext context, int jobid)
        {
            //自己的业务
            var list = await _userbussiness.QueryAsync();


            if (jobid > 0)
            {
                //任务的业务，次数
                var model = await _tasksQzServices.QueryAsync(jobid);
                if (model != null)
                {
                    model.RunTimes += 1;
                    await _tasksQzServices.UpdateAsync(model);
                }
            }

            await Console.Out.WriteLineAsync("用户总数量" + list.Count.ToString());
        }
    }
}
