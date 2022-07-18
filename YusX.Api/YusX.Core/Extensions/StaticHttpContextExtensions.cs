using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 静态HTTP上下文扩展
    /// </summary>
    public static class StaticHttpContextExtensions
    {
        /// <summary>
        /// 使用静态HTTP上下文
        /// </summary>
        /// <param name="app">应用程序</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            Utilities.HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}
