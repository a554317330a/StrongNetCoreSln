using Microsoft.AspNetCore.Mvc;
using Strong.API.AuthHelper;
using Strong.Common.Redis;
using Strong.Entities.DBModel;
using Strong.IBussiness;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strong.API.Controllers
{
    [ApiController]
    [Route("")]
    public class AccountController : ControllerBase
    {
        readonly ITB_ApilogBussiness _iTBApilogBussiness;
        private readonly IRedisCacheManager redis;
        public AccountController(ITB_ApilogBussiness _iTBApilogBussiness, IRedisCacheManager _redis)
        {
            this._iTBApilogBussiness = _iTBApilogBussiness;
            this.redis = _redis;
        }
        [HttpGet]
        public async Task<object> Login(string name, string pwd)
        {
            string jwtStr = string.Empty;
            bool suc = false;

            // 获取用户的角色名，请暂时忽略其内部是如何获取的，可以直接用 var userRole="Admin"; 来代替更好理解。
            var userRole = "Admin";//await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
            if (userRole != null)
            {
                // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
                TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = userRole };
                jwtStr = JWTHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

        [HttpGet]
        public async Task<List<TB_Apilog>> Get(int id = 1)
        {
            //IAdvertisementServices advertisementServices = new AdvertisementServices();//需要引用两个命名空间Blog.Core.IServices;Blog.Core.Services;

            //return await _iTBApilogBussiness.QueryAsync(d => d.Logid == id);
            return null;
        }

    }
}
