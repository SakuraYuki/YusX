using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YusX.Core.Configuration;
using YusX.Core.Extensions;
using YusX.Entity.System;

namespace YusX.Core.Utilities
{
    /// <summary>
    /// JWT 帮助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 生成 JWT
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public static string IssueJwt(UserInfo userInfo)
        {
            var expireMinutes = ManageUser.UserContext.MenuType == 1
                ? AppSetting.AppExpMinutes
                : AppSetting.ExpMinutes;
            var expireTimestamp = DateTime.Now.AddMinutes(expireMinutes).ToUnixTimestamp();
            var issueTimestamp = DateTimeHelper.GetUnixTimestamp();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, userInfo.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, issueTimestamp.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, issueTimestamp.ToString()) ,
                new Claim (JwtRegisteredClaimNames.Exp, expireTimestamp.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, AppSetting.Secret.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, AppSetting.Secret.Audience),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.Secret.JWT));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: AppSetting.Secret.Issuer,
                claims: claims,
                signingCredentials: creds);
            string jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return jwt;
        }

        /// <summary>
        /// 获取该 JWT 令牌过期时间
        /// </summary>
        /// <param name="jwtStr">JWT 令牌</param>
        /// <returns></returns>
        public static DateTime GetExpireTime(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            var expireTime = (jwtToken.Payload[JwtRegisteredClaimNames.Exp] ?? 0L).GetLong().UnixToDateTime();
            return expireTime;
        }

        /// <summary>
        /// 检查该 JWT 令牌是否已过期
        /// </summary>
        /// <param name="jwtStr">JWT 令牌</param>
        /// <returns></returns>
        public static bool IsExpired(string jwtStr)
            => GetExpireTime(jwtStr) < DateTime.Now;

        /// <summary>
        /// 获取该 JWT 令牌的用户ID
        /// </summary>
        /// <param name="jwtStr">JWT 令牌</param>
        /// <returns>获取失败默认返回0</returns>
        public static int GetUserId(string jwtStr)
        {
            try
            {
                return new JwtSecurityTokenHandler().ReadJwtToken(jwtStr).Id.GetInt();
            }
            catch
            {
                return 0;
            }
        }
    }
}
