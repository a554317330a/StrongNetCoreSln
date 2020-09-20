using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Common.AttributeExt
{
    /// <summary>
    ///  带有这个属性就是说明API是不用验证页面权限的方法或类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, Inherited = true)]
    public class ClientApiAttribute:Attribute
    {

    }
}
