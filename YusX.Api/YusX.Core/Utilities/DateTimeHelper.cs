using System;

namespace YusX.Core.Utilities
{
    /// <summary>
    /// 日期时间帮助类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取指定时间的友好表达文本
        /// </summary>
        /// <param name="targetDateTime">要转换的目标时间，如果传入null将返回空字符串</param>
        /// <returns>最近的日期将会返回例如今天、明天、后天、昨天或前天；较远的日期将按照yyyy-MM-dd进行格式化，例如2008-08-08</returns>
        public static string FriendlyDate(DateTime? targetDateTime)
        {
            if (!targetDateTime.HasValue)
            {
                return string.Empty;
            }

            var diffDays = (targetDateTime.Value.Date - DateTime.Now.Date).Days;

            string friendlyText;
            switch (diffDays)
            {
                case 0:
                    friendlyText = "今天";
                    break;
                case 1:
                    friendlyText = "明天";
                    break;
                case 2:
                    friendlyText = "后天";
                    break;
                case -1:
                    friendlyText = "昨天";
                    break;
                case -2:
                    friendlyText = "前天";
                    break;
                default:
                    friendlyText = targetDateTime.Value.ToString("yyyy-MM-dd");
                    break;
            }

            return friendlyText;
        }

        /// <summary>
        /// 获取当前时间的 UNIX 时间戳(秒)
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTimestamp()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取当前时间的 JavaScript 时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public static long GetJsTimestamp()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        }
    }
}

