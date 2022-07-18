using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YusX.Core.Enums;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// HTTP 上下文扩展
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取响应类型的说明消息
        /// </summary>
        /// <param name="responseType">响应类型</param>
        /// <returns>响应类型的说明消息</returns>
        public static string GetMessage(this ResponseType responseType)
        {
            var msg = responseType switch
            {
                ResponseType.LoginExpiration => "登陆已过期，请重新登陆",
                ResponseType.TokenExpiration => "令牌已过期，请重新登陆",
                ResponseType.AccountLocked => "帐号已被锁定",
                ResponseType.LoginSuccess => "登录成功",
                ResponseType.ParametersLack => "参数不完整",
                ResponseType.NoPermissions => "没有权限操作",
                ResponseType.NoRolePermissions => "角色没有权限操作",
                ResponseType.ServerError => "服务器错误",
                ResponseType.LoginError => "用户名或密码错误",
                ResponseType.SaveSuccess => "保存成功",
                ResponseType.NoKey => "没有主键不能编辑",
                ResponseType.NoKeyDel => "没有主键不能删除",
                ResponseType.KeyError => "主键不正确或没有传入主键",
                ResponseType.EidtSuccess => "编辑成功",
                ResponseType.DelSuccess => "删除成功",
                ResponseType.RegisterSuccess => "注册成功",
                ResponseType.AuditSuccess => "审核成功",
                ResponseType.ModifyPwdSuccess => "密码修改成功",
                ResponseType.OperSuccess => "操作成功",
                ResponseType.PINError => "验证码不正确",
                ResponseType.Other => "未知错误",
                _ => responseType.ToString(),
            };
            return msg;
        }

        /// <summary>
        /// 获取已注入到当前上下文的指定类型服务实例
        /// </summary>
        /// <typeparam name="T">已注册到当前上下文的服务类型</typeparam>
        /// <param name="context">HTTP 上下文</param>
        /// <returns></returns>
        public static T GetService<T>(this HttpContext context)
            where T : class
            => context.RequestServices.GetService(typeof(T)) as T;

        /// <summary>
        /// 获取请求用户的网络地址(即 IP 地址)
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns></returns>
        public static string GetUserAddress(this HttpContext context)
        {
            string realIP = null;
            string forwarded = null;
            string remoteIpAddress = context.Connection.RemoteIpAddress.ToString();
            if (context.Request.Headers.ContainsKey("X-Real-IP"))
            {
                realIP = context.Request.Headers["X-Real-IP"].ToString();
                if (realIP != remoteIpAddress)
                {
                    remoteIpAddress = realIP;
                }
            }
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                forwarded = context.Request.Headers["X-Forwarded-For"].ToString();
                if (forwarded != remoteIpAddress)
                {
                    remoteIpAddress = forwarded;
                }
            }
            return remoteIpAddress;
        }

        /// <summary>
        /// 获取请求参数字符串
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <param name="parameter">参数名</param>
        /// <returns></returns>
        public static string Request(this HttpContext context, string parameter)
        {
            try
            {
                if (context == null)
                {
                    return null;
                }

                if (context.Request.Method == "POST")
                {
                    return context.Request.Form[parameter].ToString();
                }
                else
                {
                    return context.Request.Query[parameter].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + ex.InnerException);
                return context.RequestString(parameter);
            }
        }

        /// <summary>
        /// 获取请求参数，并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">参数要序列化的类型</typeparam>
        /// <param name="context">HTTP 上下文</param>
        /// <param name="parameter">参数名</param>
        /// <returns></returns>
        public static T Request<T>(this HttpContext context, string parameter)
            where T : class
            => context.RequestString(parameter)?.DeserializeObject<T>();

        /// <summary>
        /// 获取请求参数，并得到参数系列化后的字符串
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <param name="parameter">参数名</param>
        /// <returns></returns>
        public static string RequestString(this HttpContext context, string parameter)
        {
            var requestParam = context.GetRequestParameters();
            if (string.IsNullOrWhiteSpace(requestParam))
            {
                return null;
            }

            var keyValues = requestParam.DeserializeObject<Dictionary<string, object>>();
            if (keyValues == null || keyValues.Count == 0)
            {
                return null;
            }

            if (keyValues.TryGetValue(parameter, out object value))
            {
                if (value == null)
                {
                    return null;
                }

                if (value.GetType() == typeof(string))
                {
                    return value?.ToString();
                }
                return value.Serialize();
            }
            return null;
        }

        /// <summary>
        /// 判断是否为 AJAX 请求
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpContext context)
        {
            var requestWith = context.Request.Headers == null
                ? context.Request("X-Requested-With")
                : context.Request.Headers["X-Requested-With"].ToString();

            return requestWith.EqualsIgnoreCase("XMLHttpRequest");
        }

        /// <summary>
        /// 根据 User-Agent 获取用户的操作系统平台
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns></returns>
        public static OSPlatform GetUserPlatform(this HttpContext context)
        {
            string agent = context.Request.Headers["User-Agent"].ToString().ToLower();

            if (agent.Contains("ios") || agent.Contains("iphone") || agent.Contains("ipod") || agent.Contains("ipad"))
            {
                return OSPlatform.iOS;
            }
            else if (agent.Contains("android"))
            {
                return OSPlatform.Android;
            }
            else if (agent.Contains("windows"))
            {
                return OSPlatform.Windows;
            }
            else if (agent.Contains("linux"))
            {
                return OSPlatform.Linux;
            }
            else if (agent.Contains("mac os x"))
            {
                return OSPlatform.OSX;
            }
            return OSPlatform.Other;

        }

        /// <summary>
        /// 获取请求的参数
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns></returns>

        public static string GetRequestParameters(this HttpContext context)
        {
            if (context.Request.Body == null || !context.Request.Body.CanRead || !context.Request.Body.CanSeek)
            {
                return null;
            }

            if (context.Request.Body.Length == 0)
            {
                return null;
            }

            if (context.Request.Body.Position > 0)
            {
                context.Request.Body.Position = 0;
            }

            string prarameters = null;
            var bodyStream = context.Request.Body;

            using (var buffer = new MemoryStream())
            {
                bodyStream.CopyToAsync(buffer);
                buffer.Position = 0L;
                bodyStream.Position = 0L;
                using var reader = new StreamReader(buffer, Encoding.UTF8);
                buffer.Seek(0, SeekOrigin.Begin);
                prarameters = reader.ReadToEnd();
            }
            return prarameters;
        }
    }
}
