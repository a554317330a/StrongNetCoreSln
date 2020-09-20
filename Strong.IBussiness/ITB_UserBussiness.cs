using Strong.Common.Account;
using Strong.Entities;
using Strong.Entities.DBModel;
using Strong.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Strong.IBussiness
{
    public interface ITB_UserBussiness : IBaseBussiness<TB_User>
    {
        Task<TokenModelJwt> GetUser(string name, string pwd);

        UserModel GetUserByToken(string token);

        bool GetUserPagePower(UserModel usermodel, string page);

        string getWhere(string name, string IDENTITY);

        List<TB_User> getbyredis();
 



    }
}
