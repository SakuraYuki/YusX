using YusX.Core.Extensions;

namespace YusX.Core.Providers
{
    /// <summary>
    /// Autofac容器模块
    /// </summary>
    public class DependencyProvider
    {
        /// <summary>
        /// 获取Autofac已注入的指定服务实例
        /// </summary>
        /// <typeparam name="TService">已注入的服务类型</typeparam>
        /// <returns></returns>
        public static TService GetService<TService>()
            where TService : class
        {
            return typeof(TService).GetService() as TService;
        }
    }
}
