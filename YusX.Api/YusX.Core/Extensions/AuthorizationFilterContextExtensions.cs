using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using YusX.Core.Configuration;
using YusX.Core.Enums;
using YusX.Core.Logging;
using YusX.Core.Utilities;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 授权过滤器上下文扩展
    /// </summary>
    public static class AuthorizationFilterContextExtensions
    {
        /// <summary>
        /// 设置过滤器的响应结果
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="statusCode">状态码</param>
        /// <param name="message">响应消息</param>
        /// <returns></returns>
        public static AuthorizationFilterContext FilterResult(this AuthorizationFilterContext context, HttpStatusCode statusCode, string message = null)
        {
            context.Result = new ContentResult()
            {
                Content = new
                {
                    message,
                    status = false,
                    code = (int)statusCode
                }.Serialize(),
                ContentType = "application/json",
                StatusCode = (int)statusCode
            };
            ApiLogger.Info(ApiLogType.ApiAuthorize, message);
            return context;
        }

        /// <summary>
        /// 设置表示未授权的响应结果
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="message">响应消息</param>
        /// <returns></returns>
        public static AuthorizationFilterContext Unauthorized(this AuthorizationFilterContext context, string message = null)
        {
            return context.FilterResult(HttpStatusCode.Unauthorized, message);
        }

        /// <summary>
        /// 添加指定用户的授权信息
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="userId">用户ID</param>
        public static void AddIdentity(this AuthorizationFilterContext context, int? userId = null)
        {
            userId ??= JwtHelper.GetUserId(context.HttpContext.Request.Headers[AppSetting.TokenHeaderName]);
            if (userId <= 0)
            {
                return;
            }
            //将用户Id缓存到上下文(或者自定一个对象，通过DI以AddScoped方式注入上下文来管理用户信息)
            var claims = new Claim[] { new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()) };
            context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));
        }
    }

}
