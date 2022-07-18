using YusX.Core.AutofacManager;
using YusX.Core.Providers.Path;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 服务器扩展
    /// </summary>
    public static class ServerExtensions
    {
        /// <summary>
        /// 转换相对路径到物理路径
        /// </summary>
        /// <param name="path">要转换的相对路径</param>
        /// <returns>获取到的绝对路径，末尾没有/或\\</returns>
        public static string MapPath(this string path)
        {
            return MapPath(path, false);
        }

        /// <summary>
        /// 转换相对路径到物理路径
        /// </summary>
        /// <param name="path">要转换的相对路径</param>
        /// <param name="rootPath">是否获取wwwroot路径</param>
        /// <returns>获取到的绝对路径，末尾没有/或\\</returns>
        public static string MapPath(this string path, bool rootPath)
        {
            return Ioc.GetService<IPathProvider>().MapPath(path, rootPath);
        }
    }
}
