using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
 

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YunTuCoreAPI.Controllers
{
    [ApiController]
 
    [Route("")]
    public partial class AdminUserController : BaseController
    {
        private IOptions<JsonConfig> _config;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="config"></param>
        public AdminUserController(IOptions<JsonConfig> config)
        {
            _config = config;
        }
        private readonly ITB_USERBLL userbll = new TB_USERBLL();
        private readonly ITB_TB_BUS_COMPANY companybll = new TB_BUS_COMPANYBLL();
        private readonly ITB_ROLEBLL rolebll = new TB_ROLEBLL();
        private readonly ITB_USER_ROLEBLL user_role = new TB_USER_ROLEBLL();
        /// <summary>
        /// 获取菜单的功能权限
        /// </summary>
        /// <param name="Mflag"></param>
        /// <returns></returns>
        [HttpGet]
         
        public MessageModel<List<TB_OPERATE>> getOperateByUser(string mflag)
        {
            MessageModel<List<TB_OPERATE>> result = new MessageModel<List<TB_OPERATE>>();
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var usermodel = userbll.GetUserByToken(token);
            if (usermodel == null) 
            {
                result.data = null;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "暂无数据";
                return result;
            }
            var modellist  =  new PowerMan().getOperateByUser(usermodel, mflag);

            if (modellist.Count > 0)
            {
                result.data = modellist;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = true;
                result.msg = "获取成功";
            }
            else 
            {
                result.data = null;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "暂无数据";
            }

            return result;
        }
        
        [HttpGet]
        public string GetReole()
        {
            MessageModel<DataTable> result = new MessageModel<DataTable>();
            DataSet Reole = userbll.GetReole();
            if (Reole == null)
            {
                result.data = null;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "暂无数据";
                var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return r;
            }
            else
            {
                result.data = Reole.Tables[0];
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "获取成功";
                var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return r;
            }
        }
        [HttpGet]
        public string UserDelete(int userId,string userName)
        {
            MessageModel<string> result = new MessageModel<string>();
            var usermodels = userbll.UserDelete(userId, userName, usermodel);
            if (usermodel == null)
            {
                result.data = null;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "暂无数据";
                var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return r;
            }
            else
            {
                result.data = usermodels;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "获取成功";
                var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return r;
            }

        }

        [HttpGet]
        public string  UserGetList(int page, int limit, string loginName, string userName, string unitName)
        {
            TableModel<List<TB_USER>> result = new TableModel<List<TB_USER>>();
            if (page <= 0 || limit <= 0)
            {
                result.Data = null;
                result.Code = System.Net.HttpStatusCode.OK;
                result.Success = false;
                result.Msg = "参数错误";
                result.Count = 0;
                //return result;
            }

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var usermodel = userbll.GetUserByToken(token);
            if (usermodel == null)
            {
                result.Data = null;
                result.Count = 0;
                result.Code = System.Net.HttpStatusCode.OK;
                result.Success = false;
                result.Msg = "暂无数据";
                //return result;
            }

            bool power = PowerMan.GetUserPagePower(usermodel, "user");
            if (!power)
            {
                result.Data = null;
                result.Code = System.Net.HttpStatusCode.OK;
                result.Success = false;
                result.Msg = "无权访问";
                result.Count = 0;
                
                //return result;
            }
          
            ArrayList list = new ArrayList();
            try
            {
                string strWhere = userbll.User_GetSqlWhere(loginName, userName, unitName);
                int total = 0;

                var userlist =userbll.Query(strWhere, page, limit, "sort", ref total);
                DataSet ds_rolelog = rolebll.User_Getrolelog();
                if (userlist.Count > 0)
                {
                    foreach (var item in userlist)
                    {
                        item.roleName = rolebll.User_GetRoleName(item.USERID.ToString());
                        bool bHaveLog = false;
                        DataRow[] dr = ds_rolelog.Tables[0].Select("oid='" + item.USERID.ToString() + "'");
                        if (dr.Any())
                        {
                            bHaveLog = true;
                        }
                        item.BHAVELOG = bHaveLog?"1":"0";
                    }
                    
                    result.Data = userlist;
                    result.Count = userbll.GetTotal(strWhere);
                    result.Code = System.Net.HttpStatusCode.OK;
                    result.Success = true;
                    result.Msg = "获取成功";
                    var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    return r;
                }
                else 
                {
                    result.Data = null;
                    result.Count = 0;
                    result.Code = System.Net.HttpStatusCode.OK;
                    result.Success = false;
                    result.Msg = "暂无数据";
                    //return result;
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public string GetCompany()
        {
            MessageModel<DataTable> result = new MessageModel<DataTable>();
            DataSet usermodels = companybll.GetCompany(usermodel);
            if (usermodels == null)
            {
                result.data = null;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "暂无数据";
                var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return r;
            }
            else
            {
                result.data = usermodels.Tables[0];
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "获取成功";
                var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return r;
            }

        }
        [HttpGet]
        public string AddAUserInfo(int uId)
        {
            MessageModel<ArrayList> result = new MessageModel<ArrayList>();
            ArrayList list = new ArrayList();
            DataSet user = userbll.AddAUserInfo(uId);
            string rids = "";
            if (user.Tables[0].Rows.Count>0)
            {
               
                DataSet ds = userbll.USER_ROLE(user.Tables[0].Rows[0]["USERID"].ToString());
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        rids += ds.Tables[0].Rows[i]["ROLEID"].ToString() + ",";
                    }
                    if (rids.EndsWith(","))
                        rids = rids.Substring(0, rids.Length - 1);
                }
            }
            list.Add(new
            {
                uid = user.Tables[0].Rows[0]["USERID"].ToString(),
                loginname = user.Tables[0].Rows[0]["LOGINNAME"].ToString(),
                realname = user.Tables[0].Rows[0]["REALNAME"].ToString(),
                sex = user.Tables[0].Rows[0]["SEX"].ToString() == null ? "" : user.Tables[0].Rows[0]["SEX"].ToString(),
                tel = 
                user.Tables[0].Rows[0]["TEL"].ToString() == null ? "" : user.Tables[0].Rows[0]["TEL"].ToString(),
                town = 
                user.Tables[0].Rows[0]["TOWN"].ToString() == null ? "" : user.Tables[0].Rows[0]["TOWN"].ToString(),
                unit = 
                user.Tables[0].Rows[0]["UNIT"].ToString() == null ? "" : user.Tables[0].Rows[0]["UNIT"].ToString(),
                duty = 
                user.Tables[0].Rows[0]["DUTY"].ToString() == null ? "" : user.Tables[0].Rows[0]["DUTY"].ToString(),
                email =
                user.Tables[0].Rows[0]["EMAIL"].ToString() == null ? "" : user.Tables[0].Rows[0]["EMAIL"].ToString(),
                password = DESEncrypt.Decrypt(user.Tables[0].Rows[0]["PWD"].ToString()),
                memo =
                user.Tables[0].Rows[0]["MEMO"].ToString() == null ? "" : user.Tables[0].Rows[0]["MEMO"].ToString(),
                unitid =
                user.Tables[0].Rows[0]["UNITID"].ToString() == null ? "" : user.Tables[0].Rows[0]["UNITID"].ToString(),
                rid = rids
            });
            if (list == null)
            {
                result.data = null;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "暂无数据";
                var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return r;
            }
            else
            {
                result.data = list;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = "获取成功";
                var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return r;
            }

        }
        [HttpGet]
        public string AddAUserSave(int id, string loginname, string username, string tel, string unitid, string unit, string duty,
        string email, string password, string sex, string rid, string rname, string memo, string reason)
        {
            MessageModel<string> result = new MessageModel<string>();
            TB_USER user = null;
            string  r=string.Empty;
            if (id == -1)
            {
                if (userbll.Exists(loginname))
                {
                    result.data = null;
                    result.code = System.Net.HttpStatusCode.BadRequest;
                    result.success = false;
                    result.msg = "已经存在相同名称的用户";
                    r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    return r;
                }
                user = new TB_USER();
            }
            else
            user = userbll.GetModel(id);
            user.LOGINNAME = loginname;
            user.REALNAME = username;
            user.SEX = sex == null ? "" : sex;
            user.TEL = tel == null ? "" : tel;
            user.UNITID = int.Parse(unitid);
            user.UNIT = unit == null ? "" : unit;
            user.DUTY = duty == null ? "" : duty;
            user.EMAIL = email == null ? "" : email;
            user.MEMO = memo == null ? "" : memo;
            user.PWD =DESEncrypt.Encrypt(password);
            if (id == -1)
            {
                user.ISSYSADMIN = "0";
                user.ADDTIME = DateTime.Now;
                user.CREATEUID = int.Parse(usermodel.USERID.ToString());
                int tmp = userbll.USERAdd(user);
                if (tmp > 0)
                {
                    TB_USER_ROLE bll = new TB_USER_ROLE();
                    user_role.Delete(tmp);
                    TB_ROLE_LOG rlog = new TB_ROLE_LOG();
                    TB_ROLE_LOG_PROCESS rlog_p = new TB_ROLE_LOG_PROCESS();
                    rlog.OID = tmp;
                    rlog.TYPE = "TB_USER";
                    rlog.STATUS = "0";
                    rlog.BEFORE_CONTENT = string.Empty;
                    rlog.REMARK = reason;
                    string logcontent = string.Empty;
                    string sqlremark = string.Empty;
                    logcontent = "新增用户|登录账号:" + user.LOGINNAME + ";真实姓名:" + user.REALNAME + ";性别:" + user.SEX + ";联系电话:" + user.TEL + ";所属单位:" + user.UNIT + ";邮箱:" + user.EMAIL + ";密码:" + password + ";拥有角色:" + rname + "";
                    sqlremark = "LOGINNAME:" + user.LOGINNAME + ";REALNAME:" + user.REALNAME + ";SEX:" + user.SEX + ";TEL:" + user.TEL + ";UNIT:" + user.UNIT + ";EMAIL:" + user.EMAIL + ";pwd:" + password + ";rids:" + rid + "";
                    rlog.CONTENT =  logcontent; 
                    rlog.SQLREMARK = sqlremark;;
                    string logsql = string.Empty;
                    if (rid != "")
                    {
                        string[] rids = rid.Split(',');
                        for (int i = 0; i < rids.Length; i++)
                        {
                            if (rids[i] != "" && Convert.ToInt32(rids[i]) > 0)
                            {
                                logsql += "insert into TB_USER_ROLE(USERID,ROLEID) values('" + tmp + "','" + rids[i] + "');";
                            }
                        }
                    }
                    logsql = !string.IsNullOrEmpty(logsql) ? logsql.Substring(0, logsql.Length - 1) : string.Empty;
                    rlog.SAVESQL = logsql; 
                    int logid = userbll.AddROLE_LOG(rlog);
                    string msg = string.Empty;
                    if (logid != -1)
                    {
                        rlog_p.LID = logid;
                        rlog_p.OMAN = usermodel.REALNAME;
                        rlog_p.OTIME = DateTime.Now;
                        rlog_p.REMARK = "待审批";
                        userbll.AddROLE_LOG_PROCESS(rlog_p);
                        msg = "新增成功！等待审批后生效！";
                    }
                    else
                    {
                        msg = "新增失败！";
                    }
                    result.data = null;
                    result.code = System.Net.HttpStatusCode.OK;
                    result.success = false;
                    result.msg = msg;
                    r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                }
            }
            else
            {
                TB_USER users = userbll.GetModel(id);
                List<string> list = UserEquals(users, user, rid, rname);
                string logcontent = list[0];
                string sqlremark = list[1];
                string msg = string.Empty;
                if (!string.IsNullOrEmpty(logcontent))
                {

                    TB_ROLE_LOG rlog = new TB_ROLE_LOG();
                    TB_ROLE_LOG_PROCESS rlog_p = new TB_ROLE_LOG_PROCESS();
                    rlog.OID = id;
                    rlog.TYPE = "TB_USER";
                    rlog.STATUS = "0";
                    rlog.BEFORE_CONTENT = string.Empty;
                    rlog.REMARK = reason;

                    logcontent = "编辑用户|" + logcontent;

                    rlog.CONTENT = logcontent; 
                    rlog.SQLREMARK = sqlremark; 

                    string logsql = "update TB_USER set LOGINNAME='" + user.LOGINNAME + "',PWD='" + user.PWD + "',REALNAME='" + user.REALNAME + "',SEX='" + user.SEX + "',TEL='" + user.TEL + "',UNIT='" + user.UNIT + "',DUTY='" + user.DUTY + "',EMAIL='" + user.EMAIL + "',UNITID='" + user.UNITID + "',memo='" + user.MEMO + "' where USERID=" + id + ";";

                    logsql += "delete from TB_USER_ROLE where userid=" + id + ";";
                    if (rid != ""&& rid!=null)
                    {
                        string[] rids = rid.Split(',');
                        for (int i = 0; i < rids.Length; i++)
                        {
                            if (rids[i] != "" && Convert.ToInt32(rids[i]) > 0)
                            {
                                logsql += "insert into TB_USER_ROLE(USERID,ROLEID) values('" + id + "','" + rids[i] + "');";
                            }
                        }
                    }
                    logsql = !string.IsNullOrEmpty(logsql) ? logsql.Substring(0, logsql.Length - 1) : string.Empty;
                    rlog.SAVESQL = logsql;
                    int logid = userbll.AddROLE_LOG(rlog);
                    
                    if (logid != -1)
                    {
                        rlog_p.LID = logid;
                        rlog_p.OMAN = usermodel.REALNAME;
                        rlog_p.OTIME = DateTime.Now;
                        rlog_p.REMARK = "待审批";
                        userbll.AddROLE_LOG_PROCESS(rlog_p);
                        msg = "编辑成功！等待审批后生效！";
                    }
                    else
                    {
                        msg = "编辑失败！";
                    }
                  
                }
                else
                {
                    msg = "编辑成功!";
                    
                }
                result.data = null;
                result.code = System.Net.HttpStatusCode.OK;
                result.success = false;
                result.msg = msg;
                r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            return r;
        }
        private List<string> UserEquals(TB_USER beforeuser, TB_USER user, string rids, string rnames)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            //   logcontent = "新增用户|登录账号:" + user.LOGINNAME + ";真实姓名:" + user.REALNAME + ";性别:" + user.SEX + ";手机:" + user.TEL + ";所属单位:" + user.UNIT + ";邮箱:" + user.EMAIL + ";密码:" + pwd + ";拥有角色:" + rnames + "";

            //遍历实体类属性改null为empty
            PropertyInfo[] propertys = beforeuser.GetType().GetProperties() as PropertyInfo[];
            foreach (PropertyInfo property in propertys)
            {
                if (property.GetValue(beforeuser, null) == null)
                {
                    if (property.PropertyType.Name == typeof(String).Name)
                    {
                        property.SetValue(beforeuser, string.Empty, null);
                    }
                  
                    
                }
            }

            if (!user.REALNAME.Trim().Equals(beforeuser.REALNAME.Trim())&&user.REALNAME!=null)
            {
                str += "真实姓名从'" + beforeuser.REALNAME + "'修改为'" + user.REALNAME + "';";
                str2 += "REALNAME:" + user.REALNAME + ";";
            }
            if (!user.SEX.Trim().Equals(beforeuser.SEX.Trim()))
            {
                str += "性别从'" + beforeuser.SEX + "'修改为'" + user.SEX + "';";
                str2 += "SEX:" + user.SEX + ";";
            }
            if (!user.TEL.Trim().Equals(beforeuser.TEL.Trim()))
            {
                str += "联系电话从'" + beforeuser.TEL + "'修改为'" + user.TEL + "';";
                str2 += "TEL:" + user.TEL + ";";
            }
            if (!user.UNIT.Trim().Equals(beforeuser.UNIT.Trim()))
            {
                str += "所属单位从'" + beforeuser.UNIT + "'修改为'" + user.UNIT + "';";
                str2 += "UNIT:" + user.UNIT + ";";
            }
            if (!user.EMAIL.Trim().Equals(beforeuser.EMAIL.Trim()))
            {
                str += "邮箱从'" + beforeuser.EMAIL + "'修改为'" + user.EMAIL + "';";
                str2 += "EMAIL:" + user.EMAIL + ";";
            }
            if (!user.MEMO.Trim().Equals(beforeuser.MEMO.Trim()))
            {
                str += "备注从'" + beforeuser.MEMO + "'修改为'" + user.MEMO + "';";
                str2 += "MEMO:" + user.MEMO + ";";
            }


            if (!user.PWD.Trim().Equals(beforeuser.PWD.Trim()))
            {
                str += "密码从'" +DESEncrypt.Decrypt(beforeuser.PWD) + "'修改为'" + DESEncrypt.Decrypt(user.PWD) + "';";
                str2 += "PWD:" + DESEncrypt.Decrypt(user.PWD) + ";";
            }
            DataSet ds_role = userbll.sel(int.Parse(beforeuser.USERID.ToString())); 
           

            string before_role = string.Empty;
            if (ds_role != null)
            {
                for (int i = 0; i < ds_role.Tables[0].Rows.Count; i++)
                {
                    before_role += ds_role.Tables[0].Rows[i]["rname"].ToString() + ",";
                }
            }
            if (!string.IsNullOrEmpty(before_role) && !string.IsNullOrEmpty(rnames))
            {
                before_role = before_role.Substring(0, before_role.Length - 1);
                rnames = rnames.Substring(0, rnames.Length - 1);
                string[] arr1 = before_role.Split(',');
                string[] arr2 = rnames.Split(',');
                bool change = false;
                if (arr1.Length != arr2.Length)
                {
                    change = true;
                }
                else
                {
                    for (int i = 0; i < arr2.Length; i++)
                    {
                        bool temp = false;
                        for (int j = 0; j < arr1.Length; j++)
                        {
                            if (arr2[i].Trim().Equals(arr1[j].Trim()))
                            {
                                temp = true;
                                continue;
                            }
                            if (j == arr1.Length - 1 && !temp)
                            {
                                change = true;
                            }
                        }
                    }
                }
                if (change)
                {
                    str += "拥有角色从'" + before_role + "'修改为'" + rnames + "';";
                    str2 += "rids:" + rnames + ";";
                }
            }
            List<string> list = new List<string>();
            list.Add(str);
            list.Add(str2);
            return list;
        }

        [HttpGet]
        public string GetAdminUser_spTable(string id, string atype)
        {
            TableModel<DataTable> result = new TableModel<DataTable>();
            DataTable dt = new DataTable();
            dt.Columns.Add("czlx");
            dt.Columns.Add("cznr");
            dt.Columns.Add("czyy");
            dt.Columns.Add("zt");
            string sql_temp = string.Empty;
            if (atype.Equals("TB_USER"))
            {
                sql_temp += ",(select loginname from tb_user where userid=t.oid) as name";
            }
            else
            {
                sql_temp += ",(select rname from tb_role where roleid=t.oid) as name";
            }
            DataSet ds = userbll.spTable(sql_temp,id,atype);
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string type = string.Empty;
                    string content = string.Empty;
                    string status = string.Empty;
                    object CONTENT = ds.Tables[0].Rows[i]["CONTENT"] == null ? "" : ds.Tables[0].Rows[i]["CONTENT"];
                    if (CONTENT.ToString()!="")
                    {
                        string temp_content = System.Text.Encoding.Default.GetString((byte[])CONTENT);

                        if (!string.IsNullOrEmpty(temp_content))
                        {
                            string[] strs1 = temp_content.Split('|');
                            type = strs1[0];
                            if (!string.IsNullOrEmpty(strs1[1]))
                            {
                                string[] strs2 = strs1[1].Split(';');
                                content = "<ul class=\"ultree\">";
                                for (int j = 0; j < strs2.Length; j++)
                                {
                                    content += "<li>" + strs2[j] + "</li>";
                                }
                                content += "</ul>";
                            }
                        }
                    }
                   
                    switch (ds.Tables[0].Rows[i]["status"].ToString())
                    {
                        case "-1": status = "<span style=\"color:red;\">审批不通过</span>"; break;
                        case "0": status = "<span style=\"color:blue;\">待审批</span>"; break;
                        case "1": status = "<span style=\"color:green;\">审批已通过</span>"; break;
                    }
                    //list.Add(new string[]{
                    //    (i+1).ToString(),
                    //    type,content,status,
                    //    ds.Tables[0].Rows[i]["remark"].ToString(),
                    //    ds.Tables[0].Rows[i]["name"].ToString()
                    //    });
                    DataRow dr = dt.NewRow();
                    dr["czlx"] = type;
                    dr["cznr"] = content;
                    dr["czyy"] = ds.Tables[0].Rows[i]["remark"].ToString();
                    dr["zt"] = status;
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                result.Data = dt;
                result.Count = 0;
                result.Code = System.Net.HttpStatusCode.OK;
                result.Success = false;
                result.Msg = "暂无数据";
            }
            else
            {
                result.Data = null;
                result.Count = 0;
                result.Code = System.Net.HttpStatusCode.OK;
                result.Success = false;
                result.Msg = "暂无数据";
            }
            var r = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return r;
        }

        [HttpGet]
        public string GetUserName() 
        {
            var result = new MessageModel<List<DataShareUserInfo>>();
            try
            {
               
                var model = userbll.Query().Select(o =>  new DataShareUserInfo() { Id = o.USERID, UserName = o.LOGINNAME }).ToList();
                if (model.Count > 0)
                {
                    result.data = model;
                    result.code = System.Net.HttpStatusCode.OK;
                    result.success = true;
                    result.msg = "获取成功";
                }
                else 
                {
                    result.data = null;
                    result.msg = "暂无数据";
                    result.code = System.Net.HttpStatusCode.OK;
                    result.success = true;
                }

            }
            catch (Exception ex)
            {
                result.data = null;
                result.msg = "程序错误";
                result.code = System.Net.HttpStatusCode.InternalServerError;
                result.success = false;
                 
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }


}
