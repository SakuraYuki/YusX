namespace YusX.Core.Configuration.Model
{
    /// <summary>
    /// 连接信息
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBType { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbConnectionString { get; set; }

        /// <summary>
        /// Redis 连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// 是否使用 Redis
        /// </summary>
        public bool UseRedis { get; set; }

        /// <summary>
        /// 是否使用 SignalR
        /// </summary>
        public bool UseSignalR { get; set; }
    }
}
