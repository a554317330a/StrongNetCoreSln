using Strong.Entities.DBModel;
 using Strong.IBussiness;
using Strong.IRepository.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strong.Bussiness
{
	public class TB_MenuBussiness : BaseBussiness<TB_Menu>, ITB_MenuBussiness
	{

		//当前类已经继承了增、删、查、改的方法

		//这里面写的代码不会给覆盖,如果要重新生成请删除 TB_MenuBussiness.cs


		IBaseRepository<TB_Menu> _dal;
        public TB_MenuBussiness(IBaseRepository<TB_Menu> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


		public async Task<List<TB_Menu>> GetRoleMenu(string strwhere) 
		{
			return await _dal.QueryAsync(strwhere);
		}
	}
}