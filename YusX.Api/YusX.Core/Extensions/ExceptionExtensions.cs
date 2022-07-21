using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 异常扩展
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 格式化异常
        /// </summary>
        /// <remarks>
        /// 输出格式如下：
        /// <para>【异常消息】尝试除以零。</para>
        /// <para>【异常类型】System.DivideByZeroException</para>
        /// <para>【异常方法】TestApp.TestFold.TestClass.InvokeEx(String p1, String p2)</para>
        /// <para>【异常位置】C:\Work\TestApp\TestApp\TestFold\TestClass.cs[15]</para>
        /// </remarks>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        public static string Format(this Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            var exSet = new List<(string Key, string Message)>();

            if (!string.IsNullOrWhiteSpace(ex.Message))
            {
                exSet.Add(("异常消息", ex.Message));
            }
            exSet.Add(("异常类型", ex.GetType().FullName));
            if (!string.IsNullOrWhiteSpace(ex.Source))
            {
                exSet.Add(("异常模块", ex.Source));
            }
            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                var stackTraceInfos = GetStackTraceInfo(ex.StackTrace);
                if (stackTraceInfos.Count > 0)
                {
                    var nearInfo = stackTraceInfos.First();
                    exSet.Add(("异常方法", nearInfo.Method));
                    exSet.Add(("异常源码", $"{nearInfo.Filename}[{nearInfo.LineNumber}]"));
                }
            }

            var innerEx = GetInnerException(ex, 10);
            if (innerEx == ex)
            {
                innerEx = null;
            }
            if (innerEx != null)
            {
                if (!string.IsNullOrWhiteSpace(innerEx.Message))
                {
                    exSet.Add(("实际消息", innerEx.Message));
                }
                exSet.Add(("实际类型", innerEx.GetType().FullName));
                if (!string.IsNullOrWhiteSpace(innerEx.Source))
                {
                    exSet.Add(("实际模块", innerEx.Source));
                }
                if (!string.IsNullOrWhiteSpace(innerEx.StackTrace))
                {
                    var stackTraceInfos = GetStackTraceInfo(innerEx.StackTrace);
                    if (stackTraceInfos.Count > 0)
                    {
                        var nearInfo = stackTraceInfos.First();
                        exSet.Add(("实际方法", nearInfo.Method));
                        exSet.Add(("实际源码", $"{nearInfo.Filename}[{nearInfo.LineNumber}]"));
                    }
                }
            }

            return string.Join("\n", exSet.Select(f => $"【{f.Key}】{f.Message}"));
        }

        /// <summary>
        /// 获取真实的内部异常
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="deep">最大深度</param>
        /// <returns>最终的内部异常，如果没有内部异常则返回当前异常</returns>
        public static Exception GetInnerException(this Exception ex, int deep = 10)
        {
            if (ex == null || ex.InnerException == null || deep <= 0)
            {
                return ex;
            }

            deep--;
            return GetInnerException(ex.InnerException, deep);
        }

        /// <summary>
        /// 解析堆跟踪信息
        /// </summary>
        /// <param name="stackTrace">堆跟踪信息，例：<see cref="Exception.StackTrace"/></param>
        /// <returns></returns>
        private static List<StackTraceInfo> GetStackTraceInfo(string stackTrace)
        {
            var stackTraceInfos = new List<StackTraceInfo>();

            var stackTraces = stackTrace.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in stackTraces)
            {
                var itemSp = item.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (itemSp.Length >= 5)
                {
                    var method = string.Join(" ", itemSp[1..^3]);
                    var codeFilename = itemSp[^2];
                    var codeFilenameSp = codeFilename.Split(':');
                    if (codeFilenameSp.Length > 1)
                    {
                        codeFilename = string.Join(":", codeFilenameSp[..^1]);
                    }
                    var codeFilenameLineNumber = Convert.ToInt32(itemSp[^1]);

                    stackTraceInfos.Add(new StackTraceInfo
                    {
                        Method = method,
                        Filename = codeFilename,
                        LineNumber = codeFilenameLineNumber,
                    });
                }
            }

            return stackTraceInfos;
        }
    }

    /// <summary>
    /// 堆追踪信息
    /// </summary>
    public class StackTraceInfo
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 源代码文件名
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// 源代码行号
        /// </summary>
        public int LineNumber { get; set; }
    }
}
