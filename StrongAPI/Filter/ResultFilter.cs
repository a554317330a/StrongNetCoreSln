using Microsoft.AspNetCore.Mvc.Filters;
using Strong.Entities.DBModel;
using Strong.IBussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strong.API.Filter
{
    public class ResultFilter : IAsyncResultFilter
    {
 

        private readonly ITB_ApilogBussiness _logbll;
 

        public ResultFilter( ITB_ApilogBussiness logbll) 
        {
             _logbll = logbll;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.ActionDescriptor.RouteValues["action"] != "ValidateCode")
            {
              
                var requestid = context.HttpContext.TraceIdentifier;
                var model = await _logbll.FindWhereAsync(o=>o.Requestid.Equals(requestid));
                if (model == null)
                {
                    await _logbll.AddAsync(new TB_Apilog
                    {
                        Errormsg = "日志记录错误！！！！管理员快点联系开发查看！",
                        Errortype = 3,
                        Createtime = DateTime.Now,
                        Returntime = DateTime.Now,
                        Requestid = requestid
                    });
                }
                else
                {

                    model.Returntime = DateTime.Now;
                    model.Actiontime = (decimal)Convert.ToDateTime(model.Returntime)
                        .Subtract(Convert.ToDateTime(model.Createtime)).TotalMilliseconds;
                    await _logbll.UpdateAsync(model);
                }


            }

            await next();
        }
    }
}
