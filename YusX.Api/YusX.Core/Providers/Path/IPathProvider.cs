using Microsoft.AspNetCore.Hosting;

namespace YusX.Core.Providers.Path
{
    /// <summary>
    /// 路径提供者接口
    /// </summary>
    public interface IPathProvider : IDependency
    {
        /// <summary>
        /// 转换相对路径到物理路径
        /// </summary>
        /// <param name="path">要转换的相对路径</param>
        /// <returns></returns>
        string MapPath(string path);

        /// <summary>
        /// 转换相对路径到物理路径
        /// </summary>
        /// <param name="path">要转换的相对路径</param>
        /// <param name="rootPath">是否获取wwwroot路径</param>
        /// <returns>获取到的绝对路径，末尾没有/或\\</returns>
        string MapPath(string path, bool rootPath);

        /// <summary>
        /// 获取当前寄宿环境对象
        /// </summary>
        /// <returns></returns>
        IWebHostEnvironment GetHostingEnvironment();
    }
}
