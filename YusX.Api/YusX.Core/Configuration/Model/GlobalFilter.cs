namespace YusX.Core.Configuration.Model
{
    /// <summary>
    /// 全局过滤器信息
    /// </summary>
    public class GlobalFilter
    {
        /// <summary>
        /// 过滤信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否启用过滤器
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 过滤动作集合
        /// </summary>
        public string[] Actions { get; set; }
    }
}
