using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using YusX.Core.Extensions;

namespace YusX.Core
{
    /// <summary>
    /// YusX 应用核心类
    /// </summary>
    public static class App
    {
        /// <summary>
        /// 未托管的对象集合
        /// </summary>
        public static readonly ConcurrentBag<IDisposable> UnmanagedObjects;

        /// <summary>
        /// 全局配置选项
        /// </summary>
        public static IConfiguration Config => InternalApp.Config;

        /// <summary>
        /// 获取Web主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IWebHostEnvironment WebHostEnv => InternalApp.WebHostEnv;

        /// <summary>
        /// 获取泛型主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IHostEnvironment HostEnv => InternalApp.HostEnv;

        /// <summary>
        /// 存储根服务，可能为空
        /// </summary>
        public static IServiceProvider RootServices => InternalApp.RootServices;

        /// <summary>
        /// 判断是否是单文件环境
        /// </summary>
        public static bool SingleFileEnv => string.IsNullOrWhiteSpace(Assembly.GetEntryAssembly().Location);

        /// <summary>
        /// 获取请求上下文
        /// </summary>
        public static HttpContext HttpContext => RootServices?.GetService<IHttpContextAccessor>()?.HttpContext;

        /// <summary>
        /// 获取请求上下文用户
        /// </summary>
        /// <remarks>只有授权访问的页面或接口才存在值，否则为 null</remarks>
        public static ClaimsPrincipal User => HttpContext?.User;

        /// <summary>
        /// 构造器
        /// </summary>
        static App()
        {
            // 未托管的对象
            UnmanagedObjects = new ConcurrentBag<IDisposable>();
        }

        /// <summary>
        /// 获取指定类型的服务提供器
        /// </summary>
        /// <param name="serviceType">要获取的服务类型</param>
        /// <returns></returns>
        public static IServiceProvider GetServiceProvider(Type serviceType)
        {
            // 处理控制台应用程序
            if (HostEnv == default)
            {
                return RootServices;
            }

            // 第一选择，判断是否是单例注册且单例服务不为空，如果是直接返回根服务提供器
            var isSingleton = InternalApp.Services
                .Where(u => u.ServiceType == (serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType))
                .Any(u => u.Lifetime == ServiceLifetime.Singleton);
            if (RootServices != null && isSingleton)
            {
                return RootServices;
            }

            // 第二选择是获取 HttpContext 对象的 RequestServices
            if (HttpContext?.RequestServices != null)
            {
                return HttpContext.RequestServices;
            }

            // 第三选择，创建新的作用域并返回服务提供器
            if (RootServices != null)
            {
                var scoped = RootServices.CreateScope();
                UnmanagedObjects.Add(scoped);
                return scoped.ServiceProvider;
            }

            // 第四选择，构建新的服务对象（性能最差）
            var serviceProvider = InternalApp.Services.BuildServiceProvider();
            UnmanagedObjects.Add(serviceProvider);
            return serviceProvider;
        }

        /// <summary>
        /// 获取指定类型的服务对象
        /// </summary>
        /// <typeparam name="TService">要获取的服务类型</typeparam>
        /// <param name="serviceProvider">服务提供器</param>
        /// <returns></returns>
        public static TService GetService<TService>(IServiceProvider serviceProvider = default)
            where TService : class
        {
            return GetService(typeof(TService), serviceProvider) as TService;
        }

        /// <summary>
        /// 获取指定类型的服务对象
        /// </summary>
        /// <param name="type">要获取的服务类型</param>
        /// <param name="serviceProvider">服务提供器</param>
        /// <returns></returns>
        public static object GetService(Type type, IServiceProvider serviceProvider = default)
        {
            return (serviceProvider ?? GetServiceProvider(type)).GetService(type);
        }

        /// <summary>
        /// 获取指定类型的服务对象
        /// </summary>
        /// <typeparam name="TService">要获取的服务类型</typeparam>
        /// <param name="serviceProvider">服务提供器</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">指定类型未注入时引发</exception>
        public static TService GetRequiredService<TService>(IServiceProvider serviceProvider = default)
            where TService : class
        {
            return GetRequiredService(typeof(TService), serviceProvider) as TService;
        }

        /// <summary>
        /// 获取指定类型的服务对象
        /// </summary>
        /// <param name="type">要获取的服务类型</param>
        /// <param name="serviceProvider">服务提供器</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">指定类型未注入时引发</exception>
        public static object GetRequiredService(Type type, IServiceProvider serviceProvider = default)
        {
            return (serviceProvider ?? GetServiceProvider(type)).GetRequiredService(type);
        }
    }
}
