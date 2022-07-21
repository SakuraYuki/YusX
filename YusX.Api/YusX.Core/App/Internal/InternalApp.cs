using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace YusX.Core
{
    /// <summary>
    /// YusX 内部应用核心类
    /// </summary>
    internal class InternalApp
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        internal static IServiceCollection Services;

        /// <summary>
        /// 根服务
        /// </summary>
        internal static IServiceProvider RootServices;

        /// <summary>
        /// 配置对象
        /// </summary>
        internal static IConfiguration Config;

        /// <summary>
        /// Web 主机环境
        /// </summary>
        internal static IWebHostEnvironment WebHostEnv;

        /// <summary>
        /// 通用主机环境
        /// </summary>
        internal static IHostEnvironment HostEnv;

        /// <summary>
        /// 配置 YusX 框架(Web主机)
        /// </summary>
        /// <param name="webBuilder">Web 主机构建器</param>
        /// <param name="builder">主机构建器</param>
        internal static void ConfigureApplication(IWebHostBuilder webBuilder, IHostBuilder builder = default)
        {
            // 自动装载配置
            if (builder == default)
            {
                webBuilder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    // 存储环境对象
                    HostEnv = WebHostEnv = hostContext.HostingEnvironment;
                });
            }
            // 自动装载配置
            else ConfigureHostAppConfiguration(builder);

            // 应用初始化服务
            webBuilder.ConfigureServices((hostContext, services) =>
            {
                // 存储配置对象
                Config = hostContext.Configuration;

                // 存储服务提供器
                Services = services;

                // 注册 HttpContextAccessor 服务
                services.AddHttpContextAccessor();
            });
        }

        /// <summary>
        /// 配置 YusX 框架(通用主机)
        /// </summary>
        /// <param name="builder">主机构建器</param>
        internal static void ConfigureApplication(IHostBuilder builder)
        {
            // 自动装载配置
            ConfigureHostAppConfiguration(builder);

            // 自动注入 AddApp() 服务
            builder.ConfigureServices((hostContext, services) =>
            {
                // 存储配置对象
                Config = hostContext.Configuration;

                // 存储服务提供器
                Services = services;
            });
        }

        /// <summary>
        /// 自动装载主机配置
        /// </summary>
        /// <param name="builder">主机构建器</param>
        private static void ConfigureHostAppConfiguration(IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                // 存储环境对象
                HostEnv = hostContext.HostingEnvironment;
            });
        }
    }
}
