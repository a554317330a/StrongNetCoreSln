using Strong.Entities.DBModel;
using Strong.Extensions.AttributeExt;
using Strong.IBussiness;
using Strong.IRepository.Base;
using System;
using System.Collections.Generic;
namespace Strong.Bussiness
{
	public class TB_UserBussiness : BaseBussiness<TB_User>, ITB_UserBussiness
	{

		//当前类已经继承了增、删、查、改的方法

		//这里面写的代码不会给覆盖,如果要重新生成请删除 TB_UserBussiness.cs


		IBaseRepository<TB_User> _dal;
		public TB_UserBussiness(IBaseRepository<TB_User> dal)
		{
			this._dal = dal;
			base.BaseDal = dal;
		}


		[Caching]
		public List<TB_User> getbyredis()
		{
			return _dal.Query();
		}

	}
}