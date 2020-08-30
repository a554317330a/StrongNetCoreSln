using Strong.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Strong.IBussiness
{
    public interface ITBApilogBussiness : IBaseBussiness<TbApilog, Int32>
    {
        int retrunExp();
    }
}
