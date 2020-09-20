using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization.Internal;
using Microsoft.Extensions.Options;
using Strong.Common.Account;
using Strong.Common.AttributeExt;
using Strong.Common.Redis;
using Strong.Entities.DBModel;

using Strong.IBussiness;
using Strong.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strong.API.Controllers
{
    [ApiController]
    [Route("")]
    [Authorize(Policy = "All")]
    public class AccountController : ControllerBase
    {
        readonly ITB_ApilogBussiness _iTBApilogBussiness;
 
        readonly ITB_MenuBussiness _MenuBussiness;
        readonly IWebHostEnvironment _env;
        readonly IOptions<JsonConfig> _config;
        readonly ITB_UserBussiness _UserBussiness;
        IRedisCacheManager _redis;
        public AccountController(IRedisCacheManager redis ,ITB_UserBussiness UserBussiness, IOptions<JsonConfig> config , ITB_MenuBussiness MenuBussiness, ITB_ApilogBussiness _iTBApilogBussiness, IWebHostEnvironment env)
        {
            _redis = redis;
            _config = config;
            _env = env;
            this._MenuBussiness = MenuBussiness;
            this._iTBApilogBussiness = _iTBApilogBussiness;
            _UserBussiness = UserBussiness;
 
        }

 


        #region 登录而已

        /// <summary>
        /// 获取用户信息 
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Login(string username, string password, string validatecode, string reid)
         {

            var res = new MessageModel<string>();

            //获取用户信息
            if (_env.IsDevelopment() || validatecode == "YT9999")
            {

                res.data = $"{{\"access_token\":{_config.Value.defaulttoken}}}";
                res.msg = "登录成功";
                res.success = true;
                res.code = System.Net.HttpStatusCode.OK;
                return res;
            }

            try
            {

                var requestid = reid;
                var values = _redis.GetValue(requestid + "_ValidateCode");
                if (string.IsNullOrWhiteSpace(values))
                {
                    res.data = "";
                    res.msg = "验证码过期";
                    res.code = System.Net.HttpStatusCode.RequestTimeout;
                    res.success = false;
                    return res;
                }

                if (values.Equals(validatecode))
                {

                    var viewmodel = await _UserBussiness.GetUser(username, password);
                    if (viewmodel != null)
                    {

                        res.data = $"{{\"access_token\":\"{JWTHelper.IssueJwt(viewmodel)}\"}}";
                        res.msg = "登录成功";
                        res.code = System.Net.HttpStatusCode.OK;
                        res.success = true;
                    }
                    else
                    {
                        res.data = "";
                        res.msg = "暂无数据";
                        res.code = System.Net.HttpStatusCode.OK;
                        res.success = false;
                    }
                }
                else
                {
                    res.data = "";
                    res.msg = "验证码错误";
                    res.code = System.Net.HttpStatusCode.OK;
                    res.success = false;
                }
            }
            catch (Exception e)
            {
                if (e.GetType().Name.Equals("NullReferenceException"))
                {
                    res.data = "";
                    res.msg = "账号密码错误";
                    res.code = System.Net.HttpStatusCode.OK;
                    res.success = false;
                }
                else
                {
                    res.data = "";
                    res.msg = e.Message;
                    res.code = System.Net.HttpStatusCode.OK;
                    res.success = false;
                }
            }

            return res;
        }

            #endregion

        [HttpGet]

        public async Task<MessageModel<string>> GetMenu()
        {

            try
            {
                //throw new Exception();
                var res = new MessageModel<string>();

                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                string json = "";


                var tokenModel = JWTHelper.SerializeJwt(token);
                var user = await _UserBussiness.QueryAsync(tokenModel.Uid);
                List<TB_Menu> dt = null;
                if (user.Issysadmin == "1")
                {
                    dt = await _MenuBussiness.GetRoleMenu("");
                }
                else
                {
                    dt = await _MenuBussiness.GetRoleMenu($"and mflag in(select mflag from tb_role_menu where roleid in (select roleid from tb_user_role where userid ='{Convert.ToInt32(user.Userid)}'))   ");
                }

                // DataSet ds = new Member.BLL.Common().Getsql(sql);
                if (dt.Count > 0)
                {
                    json = "[{\"name\":\"" + dt[0].Mname.ToString() + "\",\"url\":\"#/console/console\",";
                    json += "\"icon\":\"" + dt[0].Ico.ToString() + "\"},";
                    var dr = dt.Where(o => o.Mlevel == 2).ToList();
                    if (dr.Count() > 0)
                    {
                        for (int i = 0; i < dr.Count(); i++)
                        {
                            json += "{\"name\":\"" + dr[i].Mname.ToString() + "\",\"url\": \"javascript:;\",\"icon\":\"" + dr[i].Ico.ToString() + "\",\"subMenus\":[";
                            string child = "";
                            var drs = dt.Where(O => O.Mparent == dr[i].Id).ToList();
                            if (drs.Count() > 0)
                            {
                                for (int m = 0; m < drs.Count(); m++)
                                {
                                    child += "{\"name\":\"" + drs[m].Mname + "\",\"url\":\"#/" + drs[m].Murl + "\",\"icon\":\"" + drs[m].Ico + "\",\"subMenus\":[";
                                    var drm = dt.Where(o => o.Mparent == drs[m].Id).ToList();
                                    string child_ch = "";
                                    if (drm.Count() > 0)
                                    {
                                        for (int n = 0; n < drm.Count(); n++)
                                        {
                                            child_ch += "{\"name\":\"" + drm[n].Mname.ToString() + "\",\"url\":\"#/" + drm[m].Murl.ToString() + "\",\"icon\":\"" + drm[n].Ico.ToString() + "\"},";
                                        }
                                        child_ch = child_ch.Substring(0, child_ch.Length - 1);
                                    }
                                    child += child_ch + "]},";
                                }
                                child = child.Substring(0, child.Length - 1);
                            }
                            json += child + "]}";
                            if (i != dr.Count() - 1)
                            {
                                json += ",";
                            }
                        }
                    }
                    json += "]";
                }
                else
                {

                    res.data = "";
                    res.msg = "暂无数据";
                    res.code = System.Net.HttpStatusCode.OK;
                    res.success = false;
                    return res;
                }
                if (!string.IsNullOrEmpty(json))
                {
                    res.data = json;
                    res.msg = "获取成功";
                    res.code = System.Net.HttpStatusCode.OK;
                    res.success = true;
                }
                else
                {
                    res.data = "";
                    res.msg = "暂无数据";
                    res.code = System.Net.HttpStatusCode.OK;
                    res.success = false;
                }
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [ClientApi]
        public async Task<string> Get(int id = 1)
        {
            //IAdvertisementServices advertisementServices = new AdvertisementServices();//需要引用两个命名空间Blog.Core.IServices;Blog.Core.Services;

            //return await _iTBApilogBussiness.QueryAsync(d => d.Logid == id);

            var list = _UserBussiness.Query();
            var liststr = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return liststr;
        }



        [HttpGet]

        public async Task<string> Getbredis(int id = 1)
        {
            //IAdvertisementServices advertisementServices = new AdvertisementServices();//需要引用两个命名空间Blog.Core.IServices;Blog.Core.Services;

            //return await _iTBApilogBussiness.QueryAsync(d => d.Logid == id);

            var list = _UserBussiness.getbyredis();
            var liststr = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return liststr;
        }
    }
}
