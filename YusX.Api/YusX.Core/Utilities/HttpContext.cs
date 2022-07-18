using Microsoft.AspNetCore.Http;

namespace YusX.Core.Utilities
{
    /// <summary>
    /// HTTP上下文
    /// </summary>
    public static class HttpContext
    {
        /// <summary>
        /// HTTP上下文访问器
        /// </summary>
        private static IHttpContextAccessor _accessor;

        /// <summary>
        /// 当前HTTP上下文
        /// </summary>
        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        /// <summary>
        /// 使用HTTP上下文访问器配置当前HTTP上下文
        /// </summary>
        /// <param name="accessor">用于配置的HTTP上下文访问器</param>
        internal static void Configure(IHttpContextAccessor accessor)
            => _accessor = accessor;
    }
}
