using Microsoft.Extensions.Logging;
using System;

namespace YusX.Core.Logging
{
    /// <summary>
    /// 字符串日志
    /// </summary>
    public sealed class StringLogging
    {
        /// <summary>
        /// 静态缺省日志部件
        /// </summary>
        public static StringLogging Default => new StringLogging();

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level { get; private set; } = LogLevel.Information;

        /// <summary>
        /// 消息格式化参数
        /// </summary>
        public object[] Args { get; private set; }

        /// <summary>
        /// 异常对象
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 设置消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public StringLogging SetMessage(string message)
        {
            if (message != null) Message = message;
            return this;
        }

        /// <summary>
        /// 设置日志级别
        /// </summary>
        /// <param name="level">日志等级</param>
        public StringLogging SetLevel(LogLevel level)
        {
            Level = level;
            return this;
        }

        /// <summary>
        /// 设置消息格式化参数
        /// </summary>
        /// <param name="args">格式化参数</param>
        public StringLogging SetArgs(params object[] args)
        {
            if (args != null && args.Length > 0) Args = args;
            return this;
        }

        /// <summary>
        /// 设置异常对象
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        public StringLogging SetException(Exception ex)
        {
            if (ex != null) Exception = ex;
            return this;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <returns></returns>
        public void Log()
        {
            if (Message == null) return;

            var logger = App.GetService<ILoggerFactory>(App.RootServices) as ILogger;

            // 如果没有异常且事件 Id 为空
            if (Exception == null)
            {
                logger.Log(Level, Message, Args);
            }
            // 如果存在异常且事件 Id 为空
            else
            {
                logger.Log(Level, Exception, Message, Args);
            }
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Trace"/> 级别的日志
        /// </summary>
        public void LogTrace()
        {
            SetLevel(LogLevel.Trace).Log();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Debug"/> 级别的日志
        /// </summary>
        public void LogDebug()
        {
            SetLevel(LogLevel.Debug).Log();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Information"/> 级别的日志
        /// </summary>
        public void LogInformation()
        {
            SetLevel(LogLevel.Information).Log();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Warning"/> 级别的日志
        /// </summary>
        public void LogWarning()
        {
            SetLevel(LogLevel.Warning).Log();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Error"/> 级别的日志
        /// </summary>
        public void LogError()
        {
            SetLevel(LogLevel.Error).Log();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Critical"/> 级别的日志
        /// </summary>
        public void LogCritical()
        {
            SetLevel(LogLevel.Critical).Log();
        }
    }
}