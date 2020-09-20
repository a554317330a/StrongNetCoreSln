using  Strong.Entities.DBModel;
 using  Strong.IBussiness;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strong.IBussiness
{
	public interface ITB_MenuBussiness : IBaseBussiness<TB_Menu>
	{
		//当前类已经继承了增、删、查、改的方法
		//这里面写的代码不会给覆盖,如果要重新生成请删除 ITB_MenuBussiness.cs

		Task<List<TB_Menu>> GetRoleMenu(string strwhere);

	}
}