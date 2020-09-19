using Strong.Entities.DBModel;
 using Strong.IBussiness;
using Strong.IRepository.Base;
using System;
using System.Collections.Generic;
namespace Strong.Bussiness
{
	public class TB_Role_Log_ProcessBussiness : BaseBussiness<TB_Role_Log_Process>, ITB_Role_Log_ProcessBussiness
	{

        //当前类已经继承了增、删、查、改的方法

        //这里面写的代码不会给覆盖,如果要重新生成请删除 TB_Role_Log_ProcessBussiness.cs

        IBaseRepository<TB_Role_Log_Process> _dal;
        public TB_Role_Log_ProcessBussiness(IBaseRepository<TB_Role_Log_Process> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
 
	}
}