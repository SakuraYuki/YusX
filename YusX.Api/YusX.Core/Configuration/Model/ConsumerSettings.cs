namespace YusX.Core.Configuration.Model
{
    public class ConsumerSettings
    {
        public string BootstrapServers { get; set; }

        public string SaslMechanism { get; set; }

        public string SecurityProtocol { get; set; }

        public string SaslUsername { get; set; }

        public string SaslPassword { get; set; }

        public string GroupId { get; set; }
    }
}
