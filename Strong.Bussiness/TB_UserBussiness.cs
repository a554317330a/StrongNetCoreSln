using SqlSugar;
using Strong.Common.Account;
using Strong.Common.AttributeExt;
using Strong.Common.Helper;
using Strong.Entities;
using Strong.Entities.DBModel;
using Strong.IBussiness;
using Strong.IRepository;
using Strong.IRepository.Base;
using Strong.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Strong.Bussiness
{
    public class TB_UserBussiness : BaseBussiness<TB_User>, ITB_UserBussiness
    {
        readonly ITB_UserRepository _dal;
        IBaseRepository<TB_Menu> _menudal;
        IBaseRepository<TB_Role> _roledal;
        IBaseRepository<TB_User_Role> _userroledal;

        // 将多个仓储接口注入
        public TB_UserBussiness(ITB_UserRepository dal,IBaseRepository<TB_User_Role> userroledal, IBaseRepository<TB_Role> roledal, IBaseRepository<TB_Menu> menudal)
        {

            _menudal = menudal;
            _roledal = roledal;
            this._userroledal = userroledal;
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<TokenModelJwt> GetUser(string name, string pwd)
        {
            return await _dal.GetUser(name, DESEncrypt.Encrypt(pwd));
        }

        [Caching]
        public List<TB_User> getbyredis()
        {
            return _dal.Query();
        }

        /// <summary>
        /// 根据token获取用户
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserModel GetUserByToken(string token)
        {
            try
            {

                var tokenModel = JWTHelper.SerializeJwt(token);
                var userentity = _dal.Query(tokenModel.Uid);

                var usermodel = new UserModel { LOGINNAME = userentity.Loginname, DUTY = userentity.Duty, ISSYSADMIN = userentity.Issysadmin, REALNAME = userentity.Realname, UNIT = userentity.Unit, UNITID = userentity.Unitid };
                TB_User_Role usertorole = _userroledal.FindWhere(u => u.Userid == userentity.Userid);
                var role = _roledal.FindWhere(o => o.Roleid.Equals(usertorole.Roleid));
                usermodel.roleName = role.RoleIdentity;
                if (usertorole != null)
                {

                    string sqlWher = $"MFlag in (select MFlag from TB_Role_Menu where roleid={usertorole.Roleid}) and Mvisible=1 order by SORT";
                    var menuList = _menudal.SqlQuery(sqlWher);
                    Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                    foreach (DataRow item in menuList.Rows)
                    {
                        valuePairs.Add(item["MFlag"].ObjToString(), item["MNAME"].ObjToString());
                    }
                    usermodel.menudic = valuePairs;
                }
                return usermodel;

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public string getWhere(string name, string IDENTITY)
        {
            string filterStatements = "roleid <> 1";

            if (!string.IsNullOrEmpty(name))
            {
                filterStatements += " and RNAME like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(IDENTITY))
            {
                filterStatements += " and [IDENTITY] like '%" + IDENTITY + "%'";
            }

            return filterStatements;
        }

        /// <summary>
        /// 获取用户页面权限
        /// </summary>
        /// <param name="usermodel"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        public bool GetUserPagePower(UserModel usermodel, string page)
        {
            try
            {

                if (usermodel.ISSYSADMIN == "1")
                {
                    return true;
                }
                string[] arr = page.Split(';');
                bool ifPower = false;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (usermodel.menudic.ContainsKey(arr[i].Trim()) && arr[i].Trim() != "")
                    {
                        ifPower = true;
                        break;
                    }
                }
                return ifPower;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TokenModelJwt> GteUser(string username, string pwd)
        {
            return null;//await _dal.GetUser(username, pwd);
        }


    }
}
