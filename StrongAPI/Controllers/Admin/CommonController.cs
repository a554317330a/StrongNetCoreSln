using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Strong.Common.Helper;
using Strong.Common.Redis;
using Strong.Model.Common;

namespace Strong.API.Controllers
{
    [Route("")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private IRedisCacheManager _redisCacheManager;
        public CommonController(IRedisCacheManager redisCacheManager) 
        {
            _redisCacheManager = redisCacheManager; 
        }


        /// <summary>
        /// 返回验证码
        /// </summary>
        /// <returns>VilidateCodeModel</returns>
        [HttpGet]
        public MessageModel<VilidateCodeModel> ValidateCode()
        {
            try
            {

 
                var result = new MessageModel<VilidateCodeModel>();
                var model = new VilidateCodeModel();
                var obj = VerifyCodeHelper.GetSingleObj();
                var requestid = HttpContext.Request.Host.Host;
                var code = obj.CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.NumberVerifyCode);

                _redisCacheManager.SetValue(requestid + "_ValidateCode", Encoding.UTF8.GetBytes(code), TimeSpan.FromMinutes(5));
                var data = obj.CreateByteByImgVerifyCode(code, 100, 30);

                model.imgstring = $"data:image/jpeg;base64,{data}";

                model.requestip = requestid;
                result.data = model;
                result.code = HttpStatusCode.OK;
                result.msg = "获取成功";
                result.success = true;

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}

