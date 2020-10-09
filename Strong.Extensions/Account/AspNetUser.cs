using Microsoft.AspNetCore.Http;
using Strong.Common;
using Strong.IBussiness;
using Strong.Model;
using Strong.Model.ViewModel;
using System.Threading.Tasks;

namespace Strong.Extensions.Account
{
    public class AspNetUser : IUser
    {
        private readonly ITB_UserBussiness _UserBussiness;
        private readonly IHttpContextAccessor _accessor;
        public AspNetUser(IHttpContextAccessor accessor, ITB_UserBussiness UserBussiness)
        {
            _UserBussiness = UserBussiness;
            _accessor = accessor;
        }
        public string IP => GetIpAddress();


        private  string GetIpAddress ()
        {
            var ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].ObjToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = _accessor.HttpContext.Connection.RemoteIpAddress.ObjToString();
            }
            return ip;
        }

        public   UserModel UserModel
        {
            get
            {
                var token = _accessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usermodel =   _UserBussiness.GetUserByToken(token).Result;
                return usermodel;
            }
        }

    
        public string GetToken()
        {
            return _accessor.HttpContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
        }
    }
}
