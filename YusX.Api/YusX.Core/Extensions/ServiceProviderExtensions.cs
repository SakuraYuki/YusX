using System;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 服务提供者扩展
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 获取当前类型的已注入服务实例
        /// </summary>
        /// <param name="serviceType">已注入服务的类型</param>
        /// <returns></returns>
        public static object GetService(this Type serviceType)
        {
            return Utilities.HttpContext.Current.RequestServices.GetService(serviceType);
        }
    }
}
