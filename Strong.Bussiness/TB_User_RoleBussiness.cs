using Strong.Entities.DBModel;
 using Strong.IBussiness;
 using Strong.IRepository;

using System;
using System.Collections.Generic;
namespace Strong.Bussiness
{
	public class TB_User_RoleBussiness : BaseBussiness<TB_User_Role>, ITB_User_RoleBussiness
	{
 
	//当前类已经继承了增、删、查、改的方法

	//这里面写的代码不会给覆盖,如果要重新生成请删除 TB_User_RoleBussiness.cs


	ITB_User_RoleRepository  _dal;
        public TB_User_RoleBussiness(ITB_User_RoleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
 
	}
}