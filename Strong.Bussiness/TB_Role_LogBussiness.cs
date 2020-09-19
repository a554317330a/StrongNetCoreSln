using Strong.Entities.DBModel;
 using Strong.IBussiness;
using Strong.IRepository.Base;
using System;
using System.Collections.Generic;
namespace Strong.Bussiness
{
	public class TB_Role_LogBussiness : BaseBussiness<TB_Role_Log>, ITB_Role_LogBussiness
	{

		//当前类已经继承了增、删、查、改的方法

		//这里面写的代码不会给覆盖,如果要重新生成请删除 TB_Role_LogBussiness.cs


		IBaseRepository<TB_Role_Log> _dal;
        public TB_Role_LogBussiness(IBaseRepository<TB_Role_Log>  dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
 
	}
}