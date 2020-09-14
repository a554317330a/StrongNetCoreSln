using Microsoft.IdentityModel.Tokens;
using Strong.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Strong.API.AuthHelper
{
    public class JWTHelper
    {

        public static string IssueJwt(TokenModelJwt tokenModel)
        {

            string iss = Appsettings.app(new string[] { "Audience", "Issuer" });// 颁发者
            string aud = Appsettings.app(new string[] { "Audience", "Audience" });//使用者
            string secret = AppSecretConfig.Audience_Secret_String;

            //var claims = new Claim[] //old
            var claims = new List<Claim>
         {
          /*
          * 特别重要：
            1、这里将用户的部分信息，比如 uid 存到了Claim 中 ， 如果你想知道如何在其他地方将这个 uid 从 Token 中取出来，请看下边的SerializeJwt() 方法，或者在整个解决方案，搜索这个方法，看哪里使用了！
            2、你也可以研究下 HttpContext.User.Claims ，具体的你可以看看 Policys/PermissionHandler.cs 类中是如何使用的。
          */

         new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),//JWT ID,JWT的唯一标识
         new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),//IAt颁发的时间，采用标准unix时间，用于验证过期
         new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,//处理不早于这个时间的请求
         new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(3600)).ToUnixTimeSeconds()}"), //这个就是过期时间，目前是过期1000秒，可自定义，注意JWT有自己的缓冲过期时间
         new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(3600).ToString()),//过期时间
         new Claim(JwtRegisteredClaimNames.Iss,iss),//颁发者
         new Claim(JwtRegisteredClaimNames.Aud,aud),//使用者
        };

            // 可以将一个用户的多个角色全部赋予；
            claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;

        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModelJwt
            {
                Uid = (jwtToken.Id).ObjToInt(),
                Role = role != null ? role.ObjToString() : "",
            };
            return tm;
        }
    }




    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 职能
        /// </summary>
        public string Work { get; set; }

    }
}
