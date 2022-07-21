using System.IO;
using System.Text;
using YusX.Core.Configuration;
using YusX.Core.Extensions;

namespace YusX.Core.Utilities
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>存在返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        public static bool FileExists(string path)
        {
            return File.Exists(path.MapPlatformPath());
        }

        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>存在返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path.MapPlatformPath());
        }

        /// <summary>
        /// 获取当前应用的下载目录路径
        /// </summary>
        /// <returns>返回下载目录完整路径，通常为 [应用根目录]\Download\</returns>
        public static string GetCurrentDownloadPath()
        {
            return AppSetting.DownloadPath.MapPath();
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="filename">文件完整路径</param>
        /// <returns>读取到的文件内容，如果读取失败，将返回空字符串</returns>
        public static string ReadFile(string filename)
        {
            var fileFullname = filename.MapPath().MapPlatformPath();
            var readContent = string.Empty;

            if (!File.Exists(fileFullname))
            {
                return readContent;
            }

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fileFullname);
                readContent = reader.ReadToEnd();
            }
            catch
            {
            }

            reader?.Close();
            reader?.Dispose();

            return readContent;
        }

        /// <summary>
        /// 将指定内容写入文件
        /// </summary>
        /// <param name="path">文件所在目录全路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">写入的内容</param>
        /// <param name="append">是否为追加写入</param>
        /// <param name="encoding">内容编码，默认使用 <see cref="Encoding.Default"/></param>
        public static void WriteFile(string path, string fileName, string content, bool append = false, Encoding encoding = null)
        {
            path = path.MapPlatformPath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            fileName = fileName.MapPlatformPath();
            var fileFullname = Path.Combine(path, fileName).MapPlatformPath();

            using var stream = File.Open(fileFullname, FileMode.OpenOrCreate, FileAccess.Write);
            if (append)
            {
                stream.Position = stream.Length;
            }
            else
            {
                stream.SetLength(0);
            }

            var contentBytes = (encoding ?? Encoding.Default).GetBytes(content);
            stream.Write(contentBytes, 0, contentBytes.Length);
        }

        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="folderPath"></param>  
        /// <returns></returns>
        public static void DeleteFolder(string folderPath)
        {
            folderPath = folderPath.MapPlatformPath();
            if (Directory.Exists(folderPath))
            {
                foreach (var subFilePath in Directory.GetFileSystemEntries(folderPath))
                {
                    if (File.Exists(subFilePath))
                    {
                        File.Delete(subFilePath);
                    }
                    else
                    {
                        // 递归删除子文件夹
                        DeleteFolder(subFilePath);
                    }
                }
                // 删除全部文件后再删除空文件夹
                Directory.Delete(folderPath, true);
            }
        }
    }
}
