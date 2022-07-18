using Microsoft.AspNetCore.Http;
using System;

namespace YusX.Core.Services
{
    /// <summary>
    /// 接口 Action 观察者
    /// </summary>
    public class ActionObserver
    {
        /// <summary>
        /// 记录 Action 执行的开始时间
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 当前请求是否已记录日志
        /// </summary>
        public bool IsLogged { get; set; }

        /// <summary>
        /// 当前应用的 HTTP 上下文
        /// </summary>
        public HttpContext HttpContext { get; }
    }
}
