using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using YusX.Core.Constracts;
using YusX.Core.Enums;
using YusX.Core.Extensions;
using YusX.Core.Services;

namespace YusX.Core.Middleware
{
    /// <summary>
    /// 异常处理中间件
    /// </summary>
    public class ExceptionHandlerMiddleWare
    {
        /// <summary>
        /// 请求委托
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 使用请求委托进行初始化
        /// </summary>
        /// <param name="next">请求委托对象</param>
        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 调用中间件处理 HTTP 上下文
        /// </summary>
        /// <param name="context">要处理的 HTTP 上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var observer = context.RequestServices.GetService(typeof(ActionObserver)) as ActionObserver;
            try
            {
                observer.RequestTime = DateTime.Now;
                await _next(context);
                LogProvider.Info(ApiLogType.System);
                observer.IsLogged = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"服务器处理出现异常:{ex.Format()}");
                LogProvider.Error(ApiLogType.Exception, ex.Message, ex: ex);
                if (observer != null)
                {
                    observer.IsLogged = true;
                }

                context.Response.StatusCode = 500;
                context.Response.ContentType = ApplicationContentType.JSON;
                await context.Response.WriteAsync(
                    new
                    {
                        message = "~服务器没有正确处理请求,请稍等再试!",
                        status = false
                    }.Serialize(),
                    Encoding.UTF8);
            }
        }
    }
}
