using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Security.Claims;
using YusX.Core.Configuration;
using YusX.Core.Extensions;
using YusX.Core.Managers;
using YusX.Core.Utilities;

namespace YusX.Core.Filters
{
    public interface IFixedTokenFilter : IFilterMetadata
    {
        AuthorizationFilterContext OnAuthorization(AuthorizationFilterContext context);
    }

    public class FixedTokenAttribute : Attribute, IFixedTokenFilter, IAllowAnonymous
    {
        public AuthorizationFilterContext OnAuthorization(AuthorizationFilterContext context)
        {
            string fixedoken = "";
            //如果token已失效，直接获取header里的token
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                fixedoken = context.HttpContext.Request.Headers[AppSetting.TokenHeaderName];
                fixedoken = fixedoken?.Replace("Bearer ", "");
                //判断是否传入了token
                if (string.IsNullOrEmpty(fixedoken))
                {
                    return context.Unauthorized("没有传入token");
                }
                //解析token
                int userId = JwtHelper.GetUserId(fixedoken);
                if (userId <= 0)
                {
                    return context.Unauthorized("token不正确");
                }
                context.AddIdentity(userId);
            }
            else
            {
                fixedoken = ((ClaimsIdentity)context.HttpContext.User.Identity)
                ?.BootstrapContext?.ToString();
            }
            //判断当前用户的token与缓存的token是否相同
            if (UserManager.Current.Token != fixedoken)
            {
                context.Unauthorized("token已失效");
            }
            return context;
        }
    }
}
