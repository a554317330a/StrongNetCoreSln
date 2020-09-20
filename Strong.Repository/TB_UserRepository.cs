 
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlSugar;
using Strong.Common.Account;
using Strong.Entities;
using Strong.Entities.DBModel;
using Strong.IRepository;
using Strong.Repository.Base;

namespace Strong.Repository
{
    /// <summary>
    /// RoleModulePermissionRepository
    /// </summary>	
    public class TB_UserRepository : BaseRepository<TB_User>, ITB_UserRepository
    {
        public TB_UserRepository() : base()
        {
        }

        /// <summary>
        /// 获取用户信息做JWT验证
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="pwd">PWD</param>
        /// <returns>TokenModel</returns>
        public async Task<TokenModelJwt> GetUser(string name, string pwd)
        {
            var usermodel = this.EntityDB.GetSingle(o => o.Loginname.Equals(name) && o.Pwd.Equals(pwd));

            var viewmodel = await Db.Queryable<TB_Role, TB_User_Role, TB_User>((ro, ur, us) => new object[]
            {
                JoinType.Inner, ur.Roleid == ro.Roleid && ur.Userid == usermodel.Userid,
                JoinType.Inner, us.Userid == ur.Userid && us.Userid == usermodel.Userid
            }).Select((ro, ur, us) =>
                new
                {
                    Uid = (int)ur.Userid,
                    Role = ro.Rname,
                    NickName = us.Loginname
                }).ToListAsync();


            TokenModelJwt model = new TokenModelJwt();
            if (viewmodel.Count > 0)
            {
                model.Role = new List<string>();
                foreach (var item in viewmodel)
                {
                    model.Role.Add(item.Role);
                }

                model.Uid = viewmodel[0].Uid;
                model.NickName = viewmodel[0].NickName;
            }


            return model;
        }


    }

}