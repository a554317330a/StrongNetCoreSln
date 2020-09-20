
using Strong.Common.Account;
using Strong.Entities;
using Strong.Entities.DBModel;
using Strong.IRepository.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strong.IRepository
{
    /// <summary>
    /// IRoleModulePermissionRepository
    /// </summary>	
    public interface ITB_UserRepository : IBaseRepository<TB_User>//类名
    {
        Task<TokenModelJwt> GetUser(string name, string pwd);


    }
}
