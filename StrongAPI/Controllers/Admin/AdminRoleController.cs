using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SqlSugar;
using Strong.IBussiness;
using Strong.Model.Common;

namespace YunTuCoreAPI.Controllers
{
    /// <summary>
    /// 权限
    /// </summary>
    [ApiController]
    [Route("")]
    [Authorize(Policy = "All")]
    public class AdminRoleController : ControllerBase
    {

        private readonly ITB_Role_Log_ProcessBussiness _logprobll;
        private readonly ITB_Role_LogBussiness _role_logbll;
        private readonly ITB_UserBussiness _userbll;

        public AdminRoleController(ITB_Role_Log_ProcessBussiness  logprobll, ITB_Role_LogBussiness role_logbll, ITB_UserBussiness userbll) 
        {
            _userbll = userbll;
            _role_logbll = role_logbll;
            logprobll = _logprobll;
        }


        [HttpGet]
        public async Task<string> GetAdminApprovalTable(int page, int limit, string AdminApproval)
        {
            TableModel<DataTable> result = new TableModel<DataTable>();
            DataTable dt = new DataTable();
            dt.Columns.Add("czdxlx");
            dt.Columns.Add("czdx");
            dt.Columns.Add("sjzh");
            dt.Columns.Add("czlx");
            dt.Columns.Add("sqsj");
            dt.Columns.Add("sm");
            dt.Columns.Add("id");
            dt.Columns.Add("oid");
            dt.Columns.Add("type");
            string where = "STATUS='0'";
            if (!string.IsNullOrEmpty(AdminApproval))
            {
                where += " and type='" + AdminApproval + "'";
            }
   

 

            int total = 0;
            var ds = await _role_logbll.QueryAsync(where, page, limit, "ID desc");
            if (ds.Count>0)
            {

                total = await _role_logbll.GetTotalAsync(where);
                foreach (var item in ds)
                {
                    DataRow dr = dt.NewRow();
                    string temp_type = string.Empty;
                    if (item.Content!=null)
                    {
                        string temp_content = item.Content;
                       
                        if (!string.IsNullOrEmpty(temp_content))
                        {
                            string[] str = temp_content.Split('|');
                            temp_type = str[0];
                        }
                    }
                    
                    string sql_temp = string.Empty;
                    if (item.OType.ToString().Equals("TB_USER"))
                    {
                        sql_temp = "select loginname as name,realname from tb_user where userid='" + item.Oid.ToString()+ "'";
                    }
                    else
                    {
                        sql_temp += "select rname as name from tb_role where roleid='" + item.Oid.ToString() + "'";
                    }
                    DataTable ds_temp = await _role_logbll.SqlQueryAsync(sql_temp);
                    string name = string.Empty;
                    string users = "";
                    if (ds_temp != null && ds_temp.Rows.Count > 0)
                    {
                        name = ds_temp.Rows[0]["name"].ToString();

                        if (item.OType.ToString().Equals("TB_USER"))
                        {
                            users = ds_temp.Rows[0]["name"].ToString();
                        }
                    }
                    if (!item.OType.ToString().Equals("TB_USER"))
                    {
                        users = getUserByRoleId(item.Oid.ToString());
                    }
                    dr["czdxlx"] = (item.OType.ToString().Equals("TB_USER")) ? "用户" : "角色";
                    dr["czdx"] = name;
                    dr["sjzh"] = name;
                    dr["czlx"] = temp_type;
                    dr["sqsj"] = item.OperateDate == null ? "" : item.OperateDate.ToString();
                    dr["sm"] = item.Remark==null?"": item.Remark.ToString();
                    dr["id"] = item.Id.ToString();
                    dr["oid"] =  item.Oid == null ? "" : item.Oid.ToString();
                    dr["type"] = item.OType == null ? "" : item.OType.ToString();
                    dt.Rows.Add(dr);
                }
            }
            result.Data = dt;
            result.Count = total;
            result.Code = System.Net.HttpStatusCode.OK;
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
        [HttpGet]
        public string adoptmore(string ids)
        {
            MessageModel<string> result = new MessageModel<string>();
            string data = string.Empty;
            try
            {

           
            string[] arr = ids.Split(',');
            string add = "";
            for (int i = 0; i < arr.Length; i++)
            {
                add += "'" + arr[i] + "',";
            }
            add = add.Substring(0, add.Length - 1);
            for (int i = 0; i < arr.Length; i++)
            {
                List<string> AddList = new List<string>();//添加事务
                List<string> DelList = new List<string>();//删除事务
                List<string> EditList = new List<string>();//修改事务
                DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select * from TB_ROLE_LOG where id = " + arr[i] + "");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string sqls = ds.Tables[0].Rows[0]["SAVESQL"].ToString();
                    string[] arr_sql = sqls.Split(';');
                    for (int m = 0; m < arr_sql.Length; m++)
                    {
                        //CommandInfo info = new CommandInfo();
                        //info.CommandText = arr_sql[m];
                        if (arr_sql[m].StartsWith("insert"))
                        {
                            AddList.Add(arr_sql[m]);
                        }
                        else if (arr_sql[m].StartsWith("delete"))
                        {
                            DelList.Add(arr_sql[m]);
                        }
                        else if (arr_sql[m].StartsWith("update"))
                        {
                            EditList.Add(arr_sql[m]);
                        }
                    }

                    //CommandInfo update = new CommandInfo();
                    int update = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Update("update TB_ROLE_LOG set status='1' where id = " + arr[i] + "");
                    //update.CommandText = "update TB_ROLE_LOG set status='1' where id = " + arr[i] + "";
                    //EditList.Add(update);

                    int e = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().UpdateTran(EditList);
                    int d = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().UpdateTran(DelList);
                    int a = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().UpdateTran(AddList);

                    if (d > 0 && a > 0 && e > 0)
                    {
                        TB_ROLE_LOG_PROCESS rlog_p = new TB_ROLE_LOG_PROCESS();
                        rlog_p.LID = Convert.ToDecimal(arr[i]);
                        rlog_p.OMAN = usermodel.REALNAME;
                        rlog_p.OTIME = DateTime.Now;
                        rlog_p.REMARK = "审批通过";
                        role_logbll.AddTB_ROLE_LOG_PROCESS(rlog_p);
                    }
                }
            }
                data = "审批成功";
            }
            catch (Exception ex)
            {

                data = "审批失败";
            }
            result.data = data;
            result.code = System.Net.HttpStatusCode.OK;
            result.success = false;
            result.msg = "获取成功";
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);

        }
        [HttpGet]
        public string noadoptmore(string ids)
        {
            MessageModel<string> result = new MessageModel<string>();
            string data = string.Empty;
            try
            {
                //ids = ids.Substring(0, ids.Length - 1);
                string[] arr = ids.Split(',');
                string add = "";
                for (int i = 0; i < arr.Length; i++)
                {
                    add += "'" + arr[i] + "',";
                }
                add = add.Substring(0, add.Length - 1);
                List<string> DelList = new List<string>();//删除事务
                DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select * from tb_role_log where id in(" + add + ")");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string content = ds.Tables[0].Rows[i]["CONTENT"].ToString();
                        if (content.StartsWith("新增"))
                        {
                            string logsql = string.Empty;
                            if (content.StartsWith("新增用户"))
                            {

                                logsql = "delete from TB_USER where USERID='" + ds.Tables[0].Rows[i]["oid"].ToString() + "';";
                                logsql += "delete from tb_role_log_process where  lid in(select id from tb_role_log where type='TB_USER' and oid='" + ds.Tables[0].Rows[i]["oid"].ToString() + "');";
                                logsql += "delete from tb_role_log where type='TB_USER' and oid='" + ds.Tables[0].Rows[i]["oid"].ToString() + "';";
                            }
                            else if (content.StartsWith("新增角色"))
                            {

                                logsql = "delete from TB_ROLE where roleid='" + ds.Tables[0].Rows[i]["oid"].ToString() + "';";
                                logsql += "delete from tb_role_log_process where  lid in(select id from tb_role_log where type='TB_ROLE' and oid='" + ds.Tables[0].Rows[i]["oid"].ToString() + "');";
                                logsql += "delete from tb_role_log where type='TB_ROLE' and oid='" + ds.Tables[0].Rows[i]["oid"].ToString() + "';";

                            }
                            logsql = !string.IsNullOrEmpty(logsql) ? logsql.Substring(0, logsql.Length - 1) : string.Empty;
                            string[] arr_del = logsql.Split(';');

                            for (int m = 0; m < arr_del.Length; m++)
                            {
                                DelList.Add(arr_del[m].ToString());
                            }

                        }
                    }
                }
                int update = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Update("update TB_ROLE_LOG set status='-1' where id in(" + add + ")");          
                if (update > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        TB_ROLE_LOG_PROCESS rlog_p = new  TB_ROLE_LOG_PROCESS();
                        rlog_p.LID = Convert.ToDecimal(arr[i]);
                        rlog_p.OMAN = usermodel.REALNAME;
                        rlog_p.OTIME = DateTime.Now;
                        rlog_p.REMARK = "审批不通过";
                        role_logbll.AddTB_ROLE_LOG_PROCESS(rlog_p);
                        //YunTuCore.Bussiness.TB_ROLE_LOGBLL().Update(add);         
                        int ss= new YunTuCore.Bussiness.TB_ROLE_LOGBLL().UpdateTran(DelList);
                    }
                    data = "审批成功!";
                }
                else
                {
                    data = "审批失败!";
                }
            }
            catch(Exception ex)
            {
                data = "审批失败!";
            }
            result.data = data;
            result.code = System.Net.HttpStatusCode.OK;
            result.success = false;
            result.msg = "获取成功";
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
           // return JsonConvert<TableResultFormat<string>>.ObjectToJson(result);
        }
        [HttpGet]
        public string adopt(int id)
        {
            MessageModel<string> result = new MessageModel<string>();
            string data = string.Empty;
            
            List<string> AddList = new List<string>();//添加事务
            List<string> DelList = new List<string>();//删除事务
            List<string> EditList = new List<string>();//修改事务
            DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select * from TB_ROLE_LOG where id='" + id + "'");
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string sqls =ds.Tables[0].Rows[0]["SAVESQL"].ToString();
                    string[] arr_sql = sqls.Split(';');
                    for (int i = 0; i < arr_sql.Length; i++)
                    {
                        //CommandInfo info = new CommandInfo();
                        //info.CommandText = arr_sql[i];
                        if (arr_sql[i].StartsWith("insert"))
                        {
                            AddList.Add(arr_sql[i]);
                        }
                        else if (arr_sql[i].StartsWith("delete"))
                        {
                            DelList.Add(arr_sql[i]);
                        }
                        else if (arr_sql[i].StartsWith("update"))
                        {
                            EditList.Add(arr_sql[i]);
                        }
                    }
                }
            }
            //CommandInfo update = new CommandInfo();
            //update.CommandText = "update TB_ROLE_LOG set status='1' where id='" + id + "'";
            int update = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Update("update TB_ROLE_LOG set status='1' where id = " + id + "");

            int e = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().UpdateTran(EditList);
            int d = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().UpdateTran(DelList);
            int a = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().UpdateTran(AddList);

            if (d>0||a>0|| e>0)
            {
                TB_ROLE_LOG_PROCESS rlog_p = new  TB_ROLE_LOG_PROCESS();
                rlog_p.LID = id;
                rlog_p.OMAN = usermodel.REALNAME;
                rlog_p.OTIME = DateTime.Now;
                rlog_p.REMARK = "审批通过";
                role_logbll.AddTB_ROLE_LOG_PROCESS(rlog_p);
                data = "审批成功";
            }
            else
            {
                data = "审批失败";
            }
            result.data = data;
            result.code = System.Net.HttpStatusCode.OK;
            result.success = false;
            result.msg = "获取成功";
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
        [HttpGet]
        public string noadopt(int id)
        {
            MessageModel<string> result = new MessageModel<string>();
            string data = string.Empty;
            List<string> DelList = new List<string>();//删除事务
            DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select * from tb_role_log where id='" + id + "'");
            if (ds != null)
            {
                string content =ds.Tables[0].Rows[0]["CONTENT"].ToString();
                if (content.StartsWith("新增"))
                {
                    string logsql = string.Empty;
                    if (content.StartsWith("新增用户"))
                    {

                        logsql = "delete from TB_USER where USERID='" + ds.Tables[0].Rows[0]["oid"].ToString() + "';";
                        logsql += "delete from tb_role_log_process where  lid in(select id from tb_role_log where type='TB_USER' and oid='" + ds.Tables[0].Rows[0]["oid"].ToString() + "');";
                        logsql += "delete from tb_role_log where type='TB_USER' and oid='" + ds.Tables[0].Rows[0]["oid"].ToString() + "';";
                    }
                    else if (content.StartsWith("新增角色"))
                    {

                        logsql = "delete from TB_ROLE where roleid='" + ds.Tables[0].Rows[0]["oid"].ToString() + "';";
                        logsql += "delete from tb_role_log_process where  lid in(select id from tb_role_log where type='TB_ROLE' and oid='" + ds.Tables[0].Rows[0]["oid"].ToString() + "');";
                        logsql += "delete from tb_role_log where type='TB_ROLE' and oid='" + ds.Tables[0].Rows[0]["oid"].ToString() + "';";

                    }
                    logsql = !string.IsNullOrEmpty(logsql) ? logsql.Substring(0, logsql.Length - 1) : string.Empty;
                    string[] arr_del = logsql.Split(';');

                    for (int i = 0; i < arr_del.Length; i++)
                    {
                        //CommandInfo info = new CommandInfo();
                        //info.CommandText = arr_del[i];
                        DelList.Add(arr_del[i]);
                    }

                }
            }

           // string update = "update TB_ROLE_LOG set status='-1' where id='" + id + "'";
            int update = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Update("update TB_ROLE_LOG set status='-1' where id='" + id + "'");
            if (update > 0)
            {
                TB_ROLE_LOG_PROCESS rlog_p =new TB_ROLE_LOG_PROCESS();
                rlog_p.LID = id;
                rlog_p.OMAN = usermodel.REALNAME;
                rlog_p.OTIME = DateTime.Now;
                rlog_p.REMARK = "审批不通过";
                role_logbll.AddTB_ROLE_LOG_PROCESS(rlog_p);
                int ss = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().UpdateTran(DelList);
                data = "审批成功";
            }
            else
            {
                data = "审批失败";
            }
            result.data = data;
            result.code = System.Net.HttpStatusCode.OK;
            result.success = false;
            result.msg = "获取成功";
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
        private string getUserByRoleId(string id)
        {
            try
            {
                string str = "";
                string sql = "select t.loginname,t.realname from tb_user t,tb_user_role s where s.roleid=" + id + " and t.userid=s.userid";
                DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys(sql); 
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str += ds.Tables[0].Rows[i]["realname"].ToString() + "\n";
                }
                return str;
            }
            catch
            {
                return "";
            }
        }


       [HttpGet]
        public string GetList(string id, string o_type)
        {
            MessageModel<List<string[]>> result = new MessageModel<List<string[]>>();
            List<string[]> list = new List<string[]>();
            try
            {
                string sql_temp = string.Empty;
                if (o_type.Equals("TB_USER"))
                {
                    sql_temp += ",(select loginname from tb_user where userid=t.oid) as name";
                }
                else
                {
                    sql_temp += ",(select rname from tb_role where roleid=t.oid) as name";
                }
                DataTable dt = role_logbll.Query("TB_ROLE_LOG t", "", "t.id='" + id + "'", new List<SugarParameter>() { }, $"t.* {sql_temp}");// "select t.*" + sql_temp + " from TB_ROLE_LOG t  where t.id='" + id + "' and t.type='" + o_type + "' ");

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string type = string.Empty;
                        string content = string.Empty;
                        string status = string.Empty;

                        string temp_content = dt.Rows[i]["CONTENT"].ToString();
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
                        switch (dt.Rows[i]["status"].ToString())
                        {
                            case "-1": status = "<span style=\"color:red;\">审批不通过</span>"; break;
                            case "0": status = "<span style=\"color:blue;\">待审批</span>"; break;
                            case "1": status = "<span style=\"color:green;\">审批已通过</span>"; break;
                        }

                        string before_content = GetBefore(o_type, type, dt.Rows[i]["oid"].ToString());
                        string after_content = string.Empty;

                        if (!string.IsNullOrEmpty(before_content))
                        {
                            if (type.Equals("IP管理"))
                            {
                                string[] arr_remark = dt.Rows[i]["sqlremark"].ToString().Split(';');
                                string[] strs2 = before_content.Split(';');
                                before_content = "<ul class=\"ultree\">";
                                after_content = before_content;
                                for (int j = 0; j < strs2.Length; j++)
                                {

                                    before_content += "<li>" + strs2[j] + "</li>";
                                }
                                for (int j = 0; j < arr_remark.Length; j++)
                                {

                                    after_content += "<li>" + arr_remark[j] + "</li>";
                                }

                                before_content += "</ul>";
                                after_content += "</ul>";
                            }
                            else
                            {
                                string[] arr_remark = dt.Rows[i]["sqlremark"].ToString().Split(';');

                                string[] strs2 = before_content.Split(';');
                                before_content = "<ul class=\"ultree\">";
                                after_content = before_content;
                                for (int j = 0; j < strs2.Length; j++)
                                {
                                    string after_value = strs2[j].Split('|')[0];
                                    string str_tmp = strs2[j].Split('|')[1];
                                    for (int k = 0; k < arr_remark.Length; k++)
                                    {
                                        if (str_tmp == arr_remark[k].Split(':')[0])
                                        {
                                            after_value = after_value.Split(':')[0] + ":" + arr_remark[k].Split(':')[1];
                                        }
                                    }
                                    after_content += "<li><span id=\"span_" + str_tmp + "\">" + after_value + "<span></li>";
                                    before_content += "<li>" + strs2[j].Split('|')[0] + "</li>";
                                }
                                before_content += "</ul>";
                                after_content += "</ul>";
                            }
                        }
                        if (type == "分配菜单" || type == "分配功能" || type == "分配线路" || type == "分配审批功能")
                        {
                            after_content =dt.Rows[i]["sqlremark"].ToString();
                        }
                        list.Add(new string[]{
                    (i+1).ToString(),
                    type,content,status,
                    dt.Rows[i]["remark"].ToString(),
                    dt.Rows[i]["name"].ToString(),
                    before_content,after_content,
                    dt.Rows[i]["sqlremark"].ToString()
                    });
                    }
                    result.data = list;
                    result.code = System.Net.HttpStatusCode.OK;
                    result.msg = "获取成功！";
                }
                else 
                {
                    result.data = null;
                    result.code = System.Net.HttpStatusCode.BadRequest;
                    result.msg = "暂无数据！";

                }
            }
            catch (Exception e)
            {
                result.data = null;
                result.code = System.Net.HttpStatusCode.InternalServerError;
                result.msg = "内部错误！";

            }
            return JsonConvert<MessageModel<List<string[]>>>.ObjectToJson(result);
        }
      
        [HttpGet]
        public string getbmunu(string r_id)
        {
            MessageModel<DataTable> result = new MessageModel<DataTable>();
            string str = string.Empty;
            string sql = "select m.* from TB_ROLE_MENU t,tb_menu m where t.mflag=m.mflag and t.roleid=" + r_id + " order by m.mparent";
            DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                result.data = ds.Tables[0];
                result.code = System.Net.HttpStatusCode.OK;
            }
            else
            {
                result.data = null;
                result.code = System.Net.HttpStatusCode.Accepted;
            }
    
            result.success = false;
            result.msg = "获取成功";
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            
        }
        [HttpGet]
        public string getmenu(string rid, string mflags)
        {
            MessageModel<DataTable> result = new MessageModel<DataTable>();
            DataSet ds = null;
            try
            {
                string str = string.Empty;
                if (mflags.EndsWith(","))
                {
                    mflags = mflags.Substring(0, mflags.Length - 1);
                }
                string[] temp = mflags.Split(',');
                string where = string.Empty;
                for (int i = 0; i < temp.Length; i++)
                {
                    where += "'" + temp[i] + "',";
                }
                if (string.IsNullOrEmpty(where))
                {
                    return null;
                }
                else
                {
                    where = where.Substring(0, where.Length - 1);
                    string sql = "select * from tb_menu t where t.mflag in(" + where + ") order by t.mparent";
                     ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys(sql);
                   // return ds;
                }
            }
            catch
            {
                //return null;
            }
            result.data = ds.Tables[0];
            result.code = System.Net.HttpStatusCode.OK;
            result.success = false;
            result.msg = "获取成功";
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }


        [HttpGet]
        public string getbfunction(string rid, string mflag)
        {
            string str = string.Empty;
            //string sql = "select  t.*,o.*,(select mname from tb_menu where mflag=o.mflag) as mname from TB_ROLE_OPERATE t,TB_OPERATE o where t.roleid=" + rid + " and t.opkey=o.opkey and o.mflag='" + mflag + "'";
            DataTable dt = role_logbll.Query("TB_ROLE_OPERATE t,TB_OPERATE o","", " t.roleid=" + rid + " and t.opkey=o.opkey and o.mflag='" + mflag + "'", new List<SugarParameter>() { }, "t.*,o.*,(select mname from tb_menu where mflag=o.mflag) as mname");

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i.Equals(0))
                    {
                        str += dt.Rows[i]["mname"].ToString() + ":";
                    }
                    str += dt.Rows[i]["opname"].ToString() + ",";
                }
            }
            return str;
        }
        [HttpGet]
        public string getfunction(string rid, string mflag, string ops)
        {
            try
            {
                string str = string.Empty;
                ops = (ops.EndsWith(",")) ? ops.Substring(0, ops.Length - 1) : ops;
                string[] arrop = ops.Split(',');
                string where = string.Empty;
                for (int i = 0; i < arrop.Length; i++)
                {
                    where += "'" + arrop[i] + "',";
                }
                if (string.IsNullOrEmpty(where))
                {
                    return string.Empty;
                }
                else
                {
                    where = where.Substring(0, where.Length - 1);
                    //string sql = "select  o.*,(select mname from tb_menu where mflag=o.mflag) as mname from TB_OPERATE o where  o.mflag='" + mflag + "' and o.opkey in(" + where + ")";
                    //DataSet ds = Member.DBUtility.OracleHelper.Query(sql);
                    DataTable dt = role_logbll.Query("TB_OPERATE o ", "", " o.mflag='" + mflag + "' and o.opkey in(" + where + ")", new List<SugarParameter>() { }, "o.*,(select mname from tb_menu where mflag=o.mflag) as mname");

                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i.Equals(0))
                            {
                                str += dt.Rows[i]["mname"].ToString() + ":";
                            }
                            str += dt.Rows[i]["opname"].ToString() + ",";
                        }
                    }
                    return str;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        [HttpGet]
        public string getbtown(string rid)
        {
            string str = "<ul  class=\"ultree\">";
            DataTable dt = role_logbll.Query("公交线路","","",new List<SugarParameter>() { });
            DataTable ds_town = role_logbll.Query("TB_ROLE_TOWN t", "t.townid", "t.roleid='" + rid + "'",new List<SugarParameter>() { });
            for (int i = 0; i < ds_town.Rows.Count; i++)
            {
                str += "<li>";

                DataRow[] dr = dt.Select("route_code='" + ds_town.Rows[i]["townid"].ToString() + "'");
                str += dr[0]["route_name"].ToString() + (dr[0]["direction"].ToString().Equals("1") ? "(上行)" : "(下行)") + ":";


                if (ds_town.Rows[i]["PSEE"].ToString() == "1") { str += "'查看权限',"; }
                if (ds_town.Rows[i]["PADD"].ToString() == "1") { str += "'审核权限',"; }
                if (ds_town.Rows[i]["PEDIT"].ToString() == "1") { str += "'修改权限',"; }
                if (ds_town.Rows[i]["PDEL"].ToString() == "1") { str += "'查看权限',"; }
                str += "</li>";
            }
            str += "</ul>";
            return str;
        }
 
        [HttpGet]
        public string GetAdminApprovalTables( string id, string o_type,string r_id)
        {
            TableModel<ArrayList> result = new TableModel<ArrayList>();
            int total = 1;
            //List<string[]> list = new List<string[]>();
            ArrayList list = new ArrayList();
            try
            {
                string sql_temp = string.Empty;
                if (o_type.Equals("TB_USER"))
                {
                    sql_temp += ",(select loginname from tb_user where userid=t.oid) as name";
                }
                else
                {
                    sql_temp += ",(select rname from tb_role where roleid=t.oid) as name";
                }
                DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select t.*" + sql_temp + " from TB_ROLE_LOG t  where t.id='" + id + "' and t.type='" + o_type + "' ");

                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string type = string.Empty;
                        string content = string.Empty;
                        string status = string.Empty;

                        string temp_content =ds.Tables[0].Rows[i]["CONTENT"].ToString();
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
                        switch (ds.Tables[0].Rows[i]["status"].ToString())
                        {
                            case "-1": status = "<span style=\"color:red;\">审批不通过</span>"; break;
                            case "0": status = "<span style=\"color:blue;\">待审批</span>"; break;
                            case "1": status = "<span style=\"color:green;\">审批已通过</span>"; break;
                        }
                        string before_content = string.Empty;

                        before_content = GetBefore(o_type, type, ds.Tables[0].Rows[i]["oid"].ToString());
                        //string before_content = string.Empty;
                        string after_content = string.Empty;
                        if (!string.IsNullOrEmpty(before_content))
                        {
                            if (type.Equals("IP管理"))
                            {
                                string[] arr_remark = ds.Tables[0].Rows[i]["sqlremark"].ToString().Split(';');
                                string[] strs2 = before_content.Split(';');
                                before_content = "<ul class=\"ultree\">";
                                after_content = before_content;
                                for (int j = 0; j < strs2.Length; j++)
                                {

                                    before_content += "<li>" + strs2[j] + "</li>";
                                }
                                for (int j = 0; j < arr_remark.Length; j++)
                                {

                                    after_content += "<li>" + arr_remark[j] + "</li>";
                                }

                                before_content += "</ul>";
                                after_content += "</ul>";
                            }
                            else
                            {
                                string[] arr_remark =ds.Tables[0].Rows[i]["sqlremark"].ToString().Split(';');

                                string[] strs2 = before_content.Split(';');
                                before_content = "<ul class=\"ultree\">";
                                after_content = before_content;
                                for (int j = 0; j < strs2.Length; j++)
                                {
                                    string after_value = strs2[j].Split('|')[0];
                                    string str_tmp = strs2[j].Split('|')[1];
                                    for (int k = 0; k < arr_remark.Length; k++)
                                    {
                                        if (str_tmp == arr_remark[k].Split(':')[0])
                                        {
                                            after_value = after_value.Split(':')[0] + ":" + arr_remark[k].Split(':')[1];
                                        }
                                    }
                                    after_content += "<li><span id=\"span_" + str_tmp + "\">" + after_value + "<span></li>";
                                    before_content += "<li>" + strs2[j].Split('|')[0] + "</li>";
                                }
                                before_content += "</ul>";
                                after_content += "</ul>";
                            }
                        }
                        if (type == "分配菜单" )
                        {
                            string sqlremark= ds.Tables[0].Rows[i]["sqlremark"].ToString();
                            before_content = getbmunu(r_id);
                            after_content = getmenu(r_id, sqlremark);
                            //after_content = System.Text.Encoding.Default.GetString((byte[])ds.Tables[0].Rows[i]["sqlremark"]);
                        }
                        list.Add(new 
                        {
                            czlx = type,
                            xgq = before_content,
                            xgh = after_content,
                            xglr = content,
                            zt = ds.Tables[0].Rows[i]["remark"].ToString()
                        });
                        //list.Add(new string[]
                        //{
                        //    //czlx = type,
                        //    //xgq = before_content,
                        //    //xgh = after_content,
                        //    //xglr = content,
                        //    //zt = ds.Tables[0].Rows[i]["remark"].ToString()
                        //   (i + 1).ToString(),
                        //    type,
                        //    content,
                        //    status,
                        //    ds.Tables[0].Rows[i]["remark"].ToString(),
                        //    ds.Tables[0].Rows[i]["name"].ToString(),
                        //    before_content,
                        //    after_content,
                        //    System.Text.Encoding.Default.GetString((byte[])ds.Tables[0].Rows[i]["sqlremark"])
                        //});
                    }
                }
            }
            catch
            {
            }
            result.Data = list;
            result.Count = total;
            result.Code = System.Net.HttpStatusCode.OK;
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            // return "";
        }
        private string GetBefore(string action, string type, string id)
        {
            string str = string.Empty;
            if (action.Equals("TB_USER"))
            {
                if (type.Equals("新增用户") || type.Equals("删除用户"))
                {
                    str = string.Empty;
                }
                else if (type.Equals("IP管理"))
                {
                    DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select * from TB_USER_IP where user_id='" + id + "'");
                    if (ds != null)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            str += "计算机名:" + ds.Tables[0].Rows[i]["computer_name"].ToString() + ",外网地址:" + ds.Tables[0].Rows[i]["outside_net"] + ",内网地址:" + ds.Tables[0].Rows[i]["inside_net"].ToString() + ";";
                        }
                    }
                }
                else
                {
                    DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select * from TB_USER where userid='" + id + "'");
                    if (ds != null)
                    {
                        DataSet ds_role = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select (select rname from tb_role where roleid=t.roleid) as rname from TB_USER_ROLE t where t.userid=" + id + "");
                        string before_role = string.Empty;
                        if (ds_role != null)
                        {
                            for (int i = 0; i < ds_role.Tables[0].Rows.Count; i++)
                            {
                                before_role += ds_role.Tables[0].Rows[i]["rname"].ToString() + ",";
                            }
                        }
                        str = "登录账号:" + ds.Tables[0].Rows[0]["loginname"].ToString() + "|LOGINNAME;真实姓名:" + ds.Tables[0].Rows[0]["REALNAME"].ToString() + "|REALNAME;性别:" + ds.Tables[0].Rows[0]["SEX"].ToString() + "|SEX;手机:" + ds.Tables[0].Rows[0]["TEL"].ToString() + "|TEL;所属单位:" + ds.Tables[0].Rows[0]["UNIT"].ToString() + "|UNIT;邮箱:" + ds.Tables[0].Rows[0]["EMAIL"].ToString() + "|EMAIL;密码:" + DESEncrypt.Decrypt(ds.Tables[0].Rows[0]["PWD"].ToString()) + "|PWD;拥有角色:" + before_role + "|rids";
                    }
                }
            }
            else
            {
                if (type.Equals("新增角色") || type.Equals("删除角色"))
                {
                    str = string.Empty;
                }
                else
                {
                    if (type.Equals("编辑角色"))
                    {
                        DataSet ds = new YunTuCore.Bussiness.TB_ROLE_LOGBLL().Querys("select * from TB_ROLE where ROLEID='" + id + "'");
                        if (ds != null)
                        {

                            str = "角色名称:" + ds.Tables[0].Rows[0]["RNAME"].ToString() + "|RNAME;身份:" + ds.Tables[0].Rows[0]["IDENTITY"].ToString() + "|IDENTITY;角色说明:" + ds.Tables[0].Rows[0]["MEMO"].ToString() + "|MEMO";
                        }
                    }
                }
            }
            return str;
        }


        /// <summary>
        /// 获取角色对应的审核权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetRoleAuditFlow(int roleid)
        {
            MessageModel<List<RoleAuditModel>> ramodel = new MessageModel<List<RoleAuditModel>>();
            try
            {

                //(join1, join2) => new object[] { JoinType.Left, join1.UserNo == join2.UserNo }
                var modellist  = _rabll.QueryMuch<TB_AuditFlow, TB_RoleAudit, RoleAuditModel>((r1, r2) =>new object[] { JoinType.Left, r1.AuditFlowID == r2.R_AuditID && r2.R_RoleID ==roleid &&r2.IsAudit > 0},
                (r1, r2) => new RoleAuditModel() {  FlowID  = r1.AuditFlowID,  FlowName = r1.F_Name , LAY_CHECKED = SqlFunc.IIF(r2.IsAudit==1, true,false), Serial = r1.Sort});
                if (modellist.Count > 0)
                {
                    ramodel.data = modellist;
                    ramodel.code = System.Net.HttpStatusCode.OK;
                    ramodel.msg = "获取成功";
                    ramodel.success = true;
                }
                else 
                {
                    ramodel.data = null;
                    ramodel.code = System.Net.HttpStatusCode.OK;
                    ramodel.msg = "暂无数据";
                    ramodel.success = true;
                }
            }
            catch (Exception ex)
            {

                ramodel.data = null;
                ramodel.code = System.Net.HttpStatusCode.InternalServerError;
                ramodel.msg = ex.Message;
                ramodel.success = false;

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(ramodel);

        }

        /// <summary>
        ///  保存角色审核流程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string SaveRoleAudit(int roleid ,string rolename , string flowname,  string flowidls) 
        {
            MessageModel<bool> result = new MessageModel<bool>();
            try
            {
                TB_ROLE_LOG rlog = new TB_ROLE_LOG();
                TB_ROLE_LOG_PROCESS rlog_p = new TB_ROLE_LOG_PROCESS();

                //插入待处理表
                rlog.OID = roleid;
                rlog.TYPE = "TB_RoleAudit";
                rlog.STATUS = "0";
                rlog.BEFORE_CONTENT = string.Empty;
                rlog.CONTENT = $"分配审核权限|角色名称：{rolename},分配审核流程为：{flowname}";
                rlog.SQLREMARK = $"roleid:{roleid};auditid:{flowidls}.auditname:{flowname}";
                string logsql = string.Empty;
                logsql += $"delete  FROM TB_RoleAudit where R_RoleID={roleid} ;";
                var fls = flowidls.Split(',');
                for (int i = 0; i < fls.Length; i++)
                {
                    logsql += $"insert into TB_RoleAudit (R_RoleID, R_AuditID,CreateBy,IsAudit) VALUES({roleid},{fls[i]},{usermodel.USERID},1);";
                }
                logsql = !string.IsNullOrEmpty(logsql) ? logsql.Substring(0, logsql.Length - 1) : string.Empty;
                rlog.SAVESQL = logsql;

                int logid = _logbll.Add(rlog);
                if (logid != -1)
                {
                    rlog_p.LID = logid;
                    rlog_p.OMAN = usermodel.REALNAME;
                    rlog_p.OTIME = DateTime.Now;
                    rlog_p.REMARK = "待审批";
                    _logprobll.Add(rlog_p);
                    result.code = System.Net.HttpStatusCode.OK;
                    result.msg = "分配成功！等待审批后生效！";

                }
                else
                {
                    result.code = System.Net.HttpStatusCode.BadRequest;
                    result.msg = "分配失败！";
                }


            }
            catch (Exception ex)
            {
                result.data = false ;
                result.code = System.Net.HttpStatusCode.InternalServerError;
                result.msg = ex.Message;
                result.success = false;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}



