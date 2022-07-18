using System;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 日期时间扩展
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取 JavaScript 时间戳
        /// </summary>
        /// <param name="target">要转换的目标时间</param>
        /// <returns></returns>
        public static long ToJsTimestamp(this DateTime target)
            => new DateTimeOffset(target).ToUnixTimeMilliseconds();

        /// <summary>
        /// 获取 UNIX 时间戳
        /// </summary>
        /// <param name="target">要转换的目标时间</param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime target)
            => new DateTimeOffset(target).ToUnixTimeSeconds();

        /// <summary>
        /// 将毫秒值(JavaScript 时间戳)转成 C# 的 <see cref="DateTime"/> 类型
        /// </summary>
        /// <param name="target">要转换的时间戳</param>
        /// <returns></returns>
        public static DateTime JsToDateTime(this long target)
            => DateTimeOffset.FromUnixTimeMilliseconds(target).LocalDateTime;

        /// <summary>
        /// 将秒值(UNIX 时间戳)转成 C# 的 <see cref="DateTime"/> 类型
        /// </summary>
        /// <param name="target">要转换的时间戳</param>
        /// <returns></returns>
        public static DateTime UnixToDateTime(this long target)
            => DateTimeOffset.FromUnixTimeSeconds(target).LocalDateTime;
    }
}
