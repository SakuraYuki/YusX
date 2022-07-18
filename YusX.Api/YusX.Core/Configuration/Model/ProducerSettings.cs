namespace YusX.Core.Configuration.Model
{
    /// <summary>
    /// 生产者设置
    /// </summary>
    public class ProducerSettings
    {
        public string BootstrapServers { get; set; }

        public string SaslMechanism { get; set; }

        public string SecurityProtocol { get; set; }

        public string SaslUsername { get; set; }

        public string SaslPassword { get; set; }
    }
}
