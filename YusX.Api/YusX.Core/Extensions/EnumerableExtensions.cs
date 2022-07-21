using System.Collections.Generic;
using System.Linq;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 可枚举接口扩展
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 查找集合中第一个指定类型的元素
        /// </summary>
        /// <typeparam name="T">要查找的类型</typeparam>
        /// <param name="source">源集合</param>
        /// <returns>未找到时返回元素类型的默认值（一般是null）</returns>
        public static T FirstType<T>(this IEnumerable<object> source)
            where T : class
        {
            return source.FirstOrDefault(f => f is T) as T;
        }
    }
}
