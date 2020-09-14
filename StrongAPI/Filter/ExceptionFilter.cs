using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Strong.API.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {

        //private readonly ITB_MENUBLL menubll;
        //private readonly ITB_USERBLL userbll;

        //public ExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionsFilter> loggerHelper, IHubContext<ChatHub> hubContext)
        //{
        //    _env = env;
        //    _loggerHelper = loggerHelper;
        //    _hubContext = hubContext;
        //}
        public void OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
