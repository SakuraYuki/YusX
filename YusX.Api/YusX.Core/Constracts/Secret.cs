namespace YusX.Core.Constracts
{
    /// <summary>
    /// 加密对应密钥Key
    /// </summary>
    public class Secret
    {
        /// <summary>
        /// 用户密码加密 Key
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 数据库加密 Key
        /// </summary>
        public string Db { get; set; }

        /// <summary>
        /// Redis 加密 Key 
        /// </summary>
        public string Redis { get; set; }

        /// <summary>
        /// JWT 加密 Key
        /// </summary>
        public string JWT { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        /// <summary>
        /// 导出文件加密key
        /// </summary>
        public string ExportFile = "C5ABA9E202D94C13A3CB66002BF77FAF";
    }
}
