using Strong.Entities.DBModel;
 using Strong.IBussiness;
 using Strong.IRepository;
using Strong.IRepository.Base;
using System;
using System.Collections.Generic;
namespace Strong.Bussiness
{
	public class TB_JobLogBussiness : BaseBussiness<TB_JobLog>, ITB_JobLogBussiness
	{
 
	//当前类已经继承了增、删、查、改的方法

	//这里面写的代码不会给覆盖,如果要重新生成请删除 TB_JobLogBussiness.cs

		IBaseRepository<TB_JobLog> _dal;
 
        public TB_JobLogBussiness(IBaseRepository<TB_JobLog> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
 
	}
}