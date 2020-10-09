using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Strong.Common.Account;
using Strong.Entities.DBModel;
using Strong.IBussiness;
using Strong.Model.Common;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Strong.API.Filter
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public readonly ITB_ApilogBussiness _logbll;
        public ExceptionFilter(ITB_ApilogBussiness logbll) 
        {
            _logbll = logbll;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var actionname = context.ActionDescriptor.RouteValues["action"];
            var controllername = context.ActionDescriptor.RouteValues["controller"];
            var Authorization = context.HttpContext.Request.Headers["Authorization"].ToString();
            var requestid = context.HttpContext.TraceIdentifier;
            var requestmodel = await  _logbll.FindWhereAsync(o=>o.Requestid.Equals(requestid));
            if (string.IsNullOrEmpty(Authorization) && requestmodel != null)
            {

                try
                {


                    await _logbll.AddAsync(new TB_Apilog
                    {
                        Apiname = $"{controllername}-{actionname}",
                        Errormsg = $"{actionname}接口错误:{context.Exception.Message}--{context.Exception.StackTrace}",
                        Parentid = requestmodel.Logid,
                        Errortype = 2,
                        Createtime = DateTime.Now,
                        Userid = -1
                    }); ;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Authorization))
                {
                    Authorization = Authorization.Substring("Bearer ".Length).Trim();
                    var tm = JWTHelper.SerializeJwt(Authorization);
                    await _logbll.AddAsync(new TB_Apilog
                    {
                        Apiname = $"{controllername}-{actionname}",
                        Errormsg = $"{actionname}接口错误:{context.Exception.Message}--{context.Exception.StackTrace}",
                        Parentid = requestmodel.Logid,
                        Errortype = 2,
                        Userid = (int)tm.Uid
                    });
                }
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new JsonResult(new MessageModel<string>
            {
                data = null,
                msg = context.Exception.Message,
                code = HttpStatusCode.InternalServerError,
                success = false
            });
 
        }
    }
}
