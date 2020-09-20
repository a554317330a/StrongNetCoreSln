using Strong.Entities.DBModel;
 using Strong.IBussiness;
using Strong.IRepository.Base;
using System;
using System.Collections.Generic;
namespace Strong.Bussiness
{
	public class TB_ApilogBussiness : BaseBussiness<TB_Apilog>, ITB_ApilogBussiness
	{

		//当前类已经继承了增、删、查、改的方法

		//这里面写的代码不会给覆盖,如果要重新生成请删除 TB_ApilogBussiness.cs


		IBaseRepository<TB_Apilog> _dal;
        public TB_ApilogBussiness(IBaseRepository<TB_Apilog> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


     


  
 

    }
}