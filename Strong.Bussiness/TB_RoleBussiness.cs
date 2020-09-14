using Strong.Entities.DBModel;
 using Strong.IBussiness;
 using Strong.IRepository;

using System;
using System.Collections.Generic;
namespace Strong.Bussiness
{
	public class TB_RoleBussiness : BaseBussiness<TB_Role>, ITB_RoleBussiness
	{
 
	//当前类已经继承了增、删、查、改的方法

	//这里面写的代码不会给覆盖,如果要重新生成请删除 TB_RoleBussiness.cs


	ITB_RoleRepository  _dal;
        public TB_RoleBussiness(ITB_RoleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
 
	}
}