using System;
using YusX.Core.Configuration;
using YusX.Core.Enums;
using YusX.Core.Extensions;
using YusX.Core.Utilities;

namespace YusX.Core.Logging
{
    /// <summary>
    /// 文件日志记录器
    /// </summary>
    public static class FileLogger
    {
        /// <summary>
        /// 静态构造器，初次使用时初始化
        /// </summary>
        static FileLogger()
        {
        }

        /// <summary>
        /// 日志路径
        /// </summary>
        private static readonly string _logPath = System.IO.Path.Combine(AppSetting.DownloadPath, "Logs").MapPlatformPath();

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Info(string message)
        {
            RecordLog(message);
        }

        /// <summary>
        /// 记录指定类型的错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常信息</param>
        public static void Error(string message, Exception ex = null)
        {
            RecordLog(message, ex: ex);
        }

        /// <summary>
        /// 将错误内容记录到本地文件中
        /// </summary>
        /// <param name="message">错误内容</param>
        private static void RecordLog(string message, Exception ex = null)
        {
            try
            {
                var path = _logPath;
                if (ex == null)
                {
                    path = System.IO.Path.Combine(_logPath, "Errors");
                }
                FileHelper.WriteFile(
                    path: path,
                    fileName: $"{DateTime.Now:yyyyMMdd}.log",
                    content: message + Environment.NewLine,
                    append: true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"API错误日志写入文件时出错：{e.Format()}");
            }
        }
    }
}
