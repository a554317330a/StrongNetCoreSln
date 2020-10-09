
using Castle.DynamicProxy;
using log4net;
using Strong.Common.AttributeExt;
using Strong.IRepository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Strong.Extensions.AOP
{   
    public class TranAOP : IInterceptor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TranAOP));
        private readonly IUnitOfWork _unitOfWork;
        public TranAOP(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //如果需要验证
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(UseTranAttribute)) is UseTranAttribute)
            {
                try
                {
                    log.Info($"Begin Transaction");

                    _unitOfWork.BeginTran();

                    invocation.Proceed();


                    // 异步获取异常，先执行
                    if (IsAsyncMethod(invocation.Method))
                    {
                        var result = invocation.ReturnValue;
                        if (result is Task)
                        {
                            Task.WaitAll(result as Task);
                        }
                    }
                    _unitOfWork.CommitTran();

                }
                catch (Exception ex)
                {
                    log.Error($"{ex.Message.ToString()}--Rollback Transaction");
                    _unitOfWork.RollbackTran();
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }

        }

        

        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }
 


    }
}
