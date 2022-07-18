namespace YusX.Core.Configuration.Model
{
    /// <summary>
    /// 数据表默认列信息
    /// </summary>
    public abstract class TableDefaultColumns
    {
        /// <summary>
        /// 用户ID字段
        /// </summary>
        public string UserIdField { get; set; }

        /// <summary>
        /// 用户名称字段
        /// </summary>
        public string UserNameField { get; set; }

        /// <summary>
        /// 日期字段
        /// </summary>
        public string DateField { get; set; }
    }
}
