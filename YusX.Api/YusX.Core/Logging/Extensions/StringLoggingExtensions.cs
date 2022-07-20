using Microsoft.Extensions.Logging;
using System;

namespace YusX.Core.Logging.Extensions
{
    /// <summary>
    /// 字符串日志拓展
    /// </summary>
    public static class StringLoggingExtensions
    {
        /// <summary>
        /// 设置消息格式化参数
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        public static StringLogging SetArgs(this string message, params object[] args)
        {
            return new StringLogging().SetMessage(message).SetArgs(args);
        }

        /// <summary>
        /// 设置日志级别
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public static StringLogging SetLevel(this string message, LogLevel level)
        {
            return new StringLogging().SetMessage(message).SetLevel(level);
        }

        /// <summary>
        /// 设置异常对象
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象</param>
        public static StringLogging SetException(this string message, Exception ex)
        {
            return new StringLogging().SetMessage(message).SetException(ex);
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Trace"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        public static void LogTrace(this string message, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).LogTrace();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Trace"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象</param>
        /// <param name="args">格式化参数</param>
        public static void LogTrace(this string message, Exception ex, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).SetException(ex).LogTrace();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Debug"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        public static void LogDebug(this string message, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).LogDebug();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Debug"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象</param>
        /// <param name="args">格式化参数</param>
        public static void LogDebug(this string message, Exception ex, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).SetException(ex).LogDebug();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Information"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        public static void LogInformation(this string message, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).LogInformation();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Information"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象</param>
        /// <param name="args">格式化参数</param>
        public static void LogInformation(this string message, Exception ex, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).SetException(ex).LogInformation();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Warning"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        public static void LogWarning(this string message, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).LogWarning();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Warning"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象</param>
        /// <param name="args">格式化参数</param>
        public static void LogWarning(this string message, Exception ex, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).SetException(ex).LogWarning();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Error"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        public static void LogError(this string message, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).LogError();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Error"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象</param>
        /// <param name="args">格式化参数</param>
        public static void LogError(this string message, Exception ex, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).SetException(ex).LogError();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Critical"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        public static void LogCritical(this string message, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).LogCritical();
        }

        /// <summary>
        /// 记录 <see cref="LogLevel.Critical"/> 级别的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象</param>
        /// <param name="args">格式化参数</param>
        public static void LogCritical(this string message, Exception ex, params object[] args)
        {
            new StringLogging().SetMessage(message).SetArgs(args).SetException(ex).LogCritical();
        }
    }
}