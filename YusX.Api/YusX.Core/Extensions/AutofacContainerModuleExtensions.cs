using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using YusX.Core.Configuration;
using YusX.Core.Constracts;
using YusX.Core.DbAccessor;
using YusX.Core.Enums;
using YusX.Core.Providers;
using YusX.Core.Providers.Cache;
using YusX.Core.Providers.Validator;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// Autofac扩展
    /// </summary>
    public static class AutofacContainerModuleExtensions
    {
        /// <summary>
        /// 注入Autofac模块
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="builder">容器构造器</param>
        /// <param name="configuration">程序配置</param>
        /// <returns></returns>
        public static IServiceCollection AddModule(this IServiceCollection services, ContainerBuilder builder, IConfiguration configuration)
        {
            // 初始化配置文件
            AppSetting.Init(services, configuration);

            #region 注入全部可注入对象

            // 获取全部要加载的类库
            var compilationLibraries = DependencyContext.Default
                .CompileLibraries
                .Where(lib => !lib.Serviceable && lib.Type == "project")
                .ToList();

            var assemblies = new List<Assembly>();
            foreach (var compilationLib in compilationLibraries)
            {
                try
                {
                    assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(compilationLib.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(compilationLib.Name + ex.Message);
                }
            }

            var baseType = typeof(IDependency);
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            #endregion 注入全部可注入对象

            // 注入用户上下文
            builder.RegisterType<ManageUser.UserManager>().InstancePerLifetimeScope();

            // 注入接口动作检查者
            builder.RegisterType<Services.ActionObserver>().InstancePerLifetimeScope();

            // 注入模型验证器
            builder.RegisterType<ModelValidatorState>().InstancePerLifetimeScope();

            #region 注入数据库上下文

            string connectionString = DbProvider.GetConnectionString(null);
            if (DbInfo.Name.EqualsIgnoreCase(DbCurrentType.MySql))
            {
            }
            else
            {
            }

            #endregion 注入数据库上下文

            #region 注入缓存服务

            // 优先注入Redis，否则注入内存缓存
            if (AppSetting.UseRedis)
            {
                builder.RegisterType<RedisCacheService>().As<ICacheService>().SingleInstance();
            }
            else
            {
                builder.RegisterType<MemoryCacheService>().As<ICacheService>().SingleInstance();
            }

            #endregion 注入缓存服务

            return services;
        }
    }
}
