using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nancy.Json;
using Strong.Common.Account;
using Strong.Common.AttributeExt;
using Strong.Entities.DBModel;
using Strong.Extensions.Account;
using Strong.IBussiness;
using Strong.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Strong.API.Filter
{
    public class ActionFilter : IAsyncActionFilter
    {

        private readonly ITB_MenuBussiness _menubll;
        private readonly ITB_UserBussiness _userbll;

        private readonly ITB_ApilogBussiness _logbll;
        private readonly IUser _user;

        public ActionFilter( ITB_MenuBussiness menubll, ITB_UserBussiness userbll, ITB_ApilogBussiness logbll, IUser user)
        {
            _user = user;
               _menubll = menubll;
            _userbll = userbll;
            _logbll = logbll;
        }

        public class Student
        {
            public string Name { get; set; }
            public DateTime Birthday { get; set; }
        }


        ///// <summary>
        ///// 方法前记录日志
        ///// </summary>
        ///// <param name="context"></param>
        //public async override void OnActionExecuting(ActionExecutingContext context)
        //{


        //    var actionname = context.ActionDescriptor.RouteValues["action"];
        //    var controllername = context.ActionDescriptor.RouteValues["controller"];
        //    var reqArguments = Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments);
        //    var requestid = context.HttpContext.TraceIdentifier;

        //    if (actionname != "ValidateCode")
        //    {
        //        if (actionname == "Login") 
        //        {
        //            //没有用户ID
        //            var model = new TB_Apilog
        //            {
        //                Apiname = $"{controllername}-{actionname}",
        //                Createtime = DateTime.Now,
        //                Errortype = 1,
        //                Userid = -1,
        //                Params = reqArguments,
        //                Requestid = requestid,

        //            };
        //            //写日志
        //            await _logbll.AddAsync(model);
        //        }
        //        else
        //        {
        //            var Authorization = _user.GetToken();
        //            if (!string.IsNullOrWhiteSpace(Authorization))
        //            {
        //                    var tm = JWTHelper.SerializeJwt(Authorization);
        //                    if (tm == null)
        //                    {
        //                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //                        context.Result = new JsonResult(new MessageModel<string>
        //                        {
        //                            data = null,
        //                            msg = "请获取授权！",
        //                            code = HttpStatusCode.Unauthorized,
        //                            success = false
        //                        });
        //                    }
        //                    else
        //                    {
        //                        bool iswebclient = false;
        //                        iswebclient = Type.GetType(new Program().GetType().Namespace + ".Controllers." + controllername + "Controller", true, true).GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ClientApiAttribute)) as ClientApiAttribute == null;
        //                        if (iswebclient == false) 
        //                        {
        //                            iswebclient = Type.GetType(new Program().GetType().Namespace + ".Controllers." + controllername + "Controller" + actionname, true, true).GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ClientApiAttribute)) as ClientApiAttribute == null;

        //                        }

        //                        if (!iswebclient)//如果是需要判断权限的页面
        //                        {
        //                            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //                            var usermodel = _userbll.GetUserByToken(token);
        //                            var URL = context.HttpContext.Request.Headers["MFLAGURL"].ToString();
        //                            if (!string.IsNullOrEmpty(URL))
        //                            {
        //                                if (URL.Split("#").Length > 1)      //判断如果是绝对路径就不判断
        //                                {
        //                                    var menuurl = URL.Split("#")[1].Remove(0, 1);
        //                                    var menumodel = await _menubll.FindWhereAsync(o => o.Murl == menuurl);
        //                                    if (menumodel == null)
        //                                    {
        //                                        context.Result = new JsonResult(new MessageModel<string>
        //                                        {
        //                                            data = null,
        //                                            msg = "没找到页面！",
        //                                            code = HttpStatusCode.NotFound,
        //                                            success = false
        //                                        });
        //                                    }
        //                                    else
        //                                    {
        //                                        if (menumodel.Mflag != null)
        //                                        {
        //                                            var power = _userbll.GetUserPagePower(usermodel, menumodel.Mflag);
        //                                            if (!power)
        //                                            {
        //                                                context.Result = new JsonResult(new MessageModel<string>
        //                                                {
        //                                                    data = null,
        //                                                    msg = "没有权限！",
        //                                                    code = HttpStatusCode.Forbidden,
        //                                                    success = false
        //                                                });
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            context.Result = new JsonResult(new MessageModel<string>
        //                                            {
        //                                                data = null,
        //                                                msg = "没找到页面！",
        //                                                code = HttpStatusCode.NotFound,
        //                                                success = false
        //                                            });
        //                                        }
        //                                    }

        //                                }

        //                            }
        //                            else
        //                            {
        //                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        //                            context.Result = new JsonResult(new MessageModel<string>
        //                                {
        //                                    data = null,
        //                                    msg = "请从登录页面进入！",
        //                                    code = HttpStatusCode.NotFound,
        //                                    success = false
        //                                });
        //                            }
        //                        }
        //                        var model = new TB_Apilog
        //                        {
        //                            Apiname = $"{controllername}-{actionname}",
        //                            Createtime = DateTime.Now,
        //                            Errortype = 1,
        //                            Userid = (int)tm.Uid,
        //                            Params = reqArguments,
        //                            Requestid = requestid,
        //                        };
                               
        //                        //写日志
        //                        await _logbll.AddAsync(model);
        //                        //DbCollection.GetDb.Insertable<TB_APILOG>(model);//

        //                    }
                        
        //            }
        //            else
        //            {
        //                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //                context.Result = new JsonResult(new MessageModel<string>
        //                {
        //                    data = null,
        //                    msg = "请获取授权！",
        //                    code = HttpStatusCode.Unauthorized,
        //                    success = false
        //                });


        //            }
        //        }
        //    }
        //  await base.OnActionExecutionAsync(context);
        //}

        //public  async  override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    if (context.ActionDescriptor.RouteValues["action"] != "ValidateCode")
        //    {
             
        //        var requestid = context.HttpContext.TraceIdentifier;
        //        var model = await _logbll.QueryAsync(o => o.Requestid.Equals(requestid)).Count == 1 ? _logbll.Query(o => o.Requestid.Equals(requestid))[0] : null;
        //        if (model == null)
        //        {
        //            await _logbll.AddAsync(new TB_Apilog
        //            {
        //                Errormsg = "日志记录错误！！！！管理员快点联系开发查看！",
        //                Errortype = 3,
        //                Createtime = DateTime.Now,
        //                Returntime = DateTime.Now,
        //                Requestid = requestid
        //            });
        //        }
        //        else
        //        {

        //            model.Returntime = DateTime.Now;
        //            model.Actiontime = (decimal)Convert.ToDateTime(model.Returntime)
        //                .Subtract(Convert.ToDateTime(model.Createtime)).TotalMilliseconds;
        //            var aa = _logbll.UpdateAsync(model);
        //        }


        //    }

        //    base.OnResultExecutionAsync(context);
        //}

        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

 



            var actionname = context.ActionDescriptor.RouteValues["action"];
            var controllername = context.ActionDescriptor.RouteValues["controller"];
            var reqArguments = Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments);
            var requestid = context.HttpContext.TraceIdentifier;

            if (actionname != "ValidateCode")
            {
                if (actionname == "Login")
                {
                    //没有用户ID
                    var model = new TB_Apilog
                    {
                        Apiname = $"{controllername}-{actionname}",
                        Createtime = DateTime.Now,
                        Errortype = 1,
                        Userid = -1,
                        Params = reqArguments,
                        Requestid = requestid,

                    };
                    //写日志
                    await _logbll.AddAsync(model);
                }
                else
                {
                    var Authorization = _user.GetToken();
                    if (!string.IsNullOrWhiteSpace(Authorization))
                    {
                        var tm = JWTHelper.SerializeJwt(Authorization);
                        if (tm == null)
                        {
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Result = new JsonResult(new MessageModel<string>
                            {
                                data = null,
                                msg = "请获取授权！",
                                code = HttpStatusCode.Unauthorized,
                                success = false
                            });
                        }
                        else
                        {
                            bool iswebclient = false;
                            iswebclient = Type.GetType(new Program().GetType().Namespace + ".Controllers." + controllername + "Controller", true, true).GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ClientApiAttribute)) as ClientApiAttribute == null;
                            if (iswebclient == false)
                            {
                                iswebclient = Type.GetType(new Program().GetType().Namespace + ".Controllers." + controllername + "Controller" + actionname, true, true).GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ClientApiAttribute)) as ClientApiAttribute == null;

                            }

                            if (!iswebclient)//如果是需要判断权限的页面
                            {
                                var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                                var usermodel = await _userbll.GetUserByToken(token);
                                var URL = context.HttpContext.Request.Headers["MFLAGURL"].ToString();
                                if (!string.IsNullOrEmpty(URL))
                                {
                                    if (URL.Split("#").Length > 1)      //判断如果是绝对路径就不判断
                                    {
                                        var menuurl = URL.Split("#")[1].Remove(0, 1);
                                        var menumodel = await _menubll.FindWhereAsync(o => o.Murl == menuurl);
                                        if (menumodel == null)
                                        {
                                            context.Result = new JsonResult(new MessageModel<string>
                                            {
                                                data = null,
                                                msg = "没找到页面！",
                                                code = HttpStatusCode.NotFound,
                                                success = false
                                            });
                                        }
                                        else
                                        {
                                            if (menumodel.Mflag != null)
                                            {
                                                var power = _userbll.GetUserPagePower(usermodel, menumodel.Mflag);
                                                if (!power)
                                                {
                                                    context.Result = new JsonResult(new MessageModel<string>
                                                    {
                                                        data = null,
                                                        msg = "没有权限！",
                                                        code = HttpStatusCode.Forbidden,
                                                        success = false
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                context.Result = new JsonResult(new MessageModel<string>
                                                {
                                                    data = null,
                                                    msg = "没找到页面！",
                                                    code = HttpStatusCode.NotFound,
                                                    success = false
                                                });
                                            }
                                        }

                                    }

                                }
                                else
                                {
                                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                    context.Result = new JsonResult(new MessageModel<string>
                                    {
                                        data = null,
                                        msg = "请从登录页面进入！",
                                        code = HttpStatusCode.NotFound,
                                        success = false
                                    });
                                }
                            }
                            var model = new TB_Apilog
                            {
                                Apiname = $"{controllername}-{actionname}",
                                Createtime = DateTime.Now,
                                Errortype = 1,
                                Userid = (int)tm.Uid,
                                Params = reqArguments,
                                Requestid = requestid,
                            };

                            //写日志
                            await _logbll.AddAsync(model);
                            //DbCollection.GetDb.Insertable<TB_APILOG>(model);//

                        }

                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Result = new JsonResult(new MessageModel<string>
                        {
                            data = null,
                            msg = "请获取授权！",
                            code = HttpStatusCode.Unauthorized,
                            success = false
                        });


                    }
                }
            }

            await next();
        }
    }
}
