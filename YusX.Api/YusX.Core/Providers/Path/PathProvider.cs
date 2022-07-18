using Microsoft.AspNetCore.Hosting;
using YusX.Core.Extensions;

namespace YusX.Core.Providers.Path
{
    /// <summary>
    /// 路径提供者
    /// </summary>
    public class PathProvider : IPathProvider
    {
        /// <summary>
        /// 当前寄宿环境
        /// </summary>
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// 使用指定的寄宿环境对象初始化
        /// </summary>
        /// <param name="environment">寄宿环境</param>
        public PathProvider(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public string MapPath(string path)
        {
            return MapPath(path, false);
        }

        public string MapPath(string path, bool rootPath)
        {
            if (rootPath)
            {
                if (_hostingEnvironment.WebRootPath == null)
                {
                    _hostingEnvironment.WebRootPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot").MapPlatformPath();
                }
                return Path.Combine(_hostingEnvironment.WebRootPath, path).MapPlatformPath();
            }
            return Path.Combine(_hostingEnvironment.ContentRootPath, path).MapPlatformPath();
        }

        public IWebHostEnvironment GetHostingEnvironment()
        {
            return _hostingEnvironment;
        }
    }
}
