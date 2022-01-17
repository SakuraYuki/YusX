using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YusX.Entity.Mappings
{
    /// <summary>
    /// 实体映射配置基础接口
    /// </summary>
    public interface IEntityMappingConfiguration
    {
        /// <summary>
        /// 创建映射配置
        /// </summary>
        /// <param name="builder">模型构建器</param>
        void Map(ModelBuilder builder);
    }

    /// <summary>
    /// 实体映射配置接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IEntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class
    {
        /// <summary>
        /// 创建映射配置
        /// </summary>
        /// <param name="builder">模型构建器</param>
        void Map(EntityTypeBuilder<T> builder);
    }

    /// <summary>
    /// 实体映射配置基类
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration<T> where T : class
    {
        public abstract void Map(EntityTypeBuilder<T> b);

        public void Map(ModelBuilder b)
        {
            Map(b.Entity<T>());
        }
    }

    /// <summary>
    /// 模型构建器扩展
    /// </summary>
    public static class ModelBuilderExtenions
    {
        /// <summary>
        /// 获取程序集中指定映射类型类型的全部子类类型
        /// </summary>
        /// <param name="assembly">要查找的程序集</param>
        /// <param name="mappingInterface">限定的父类型</param>
        /// <returns></returns>
        private static IEnumerable<Type> GetMappingTypes(this Assembly assembly, Type mappingInterface)
        {
            return assembly.GetTypes()
                .Where(x => !x.IsAbstract &&
                    x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType &&
                    y.GetGenericTypeDefinition() == mappingInterface)
                );
        }

        /// <summary>
        /// 从程序集中查找所有的映射配置并应用
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="assembly">程序集</param>
        public static void AddEntityConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var mappingTypes = assembly.GetMappingTypes(typeof(IEntityMappingConfiguration<>));
            foreach (var config in mappingTypes.Select(Activator.CreateInstance).Cast<IEntityMappingConfiguration>())
            {
                config.Map(modelBuilder);
            }
        }
    }
}

