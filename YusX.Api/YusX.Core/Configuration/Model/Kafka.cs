namespace YusX.Core.Configuration.Model
{
    /// <summary>
    /// Kafka配置信息
    /// </summary>
    public class Kafka
    {
        /// <summary>
        /// 是否使用生产者
        /// </summary>
        public bool UseProducer { get; set; }

        /// <summary>
        /// 生产者设置
        /// </summary>
        public ProducerSettings ProducerSettings { get; set; }

        /// <summary>
        /// 是否使用消费者
        /// </summary>
        public bool UseConsumer { get; set; }

        /// <summary>
        /// 消费者是否订阅
        /// </summary>
        public bool IsConsumerSubscribe { get; set; }

        /// <summary>
        /// 消费者设置
        /// </summary>
        public ConsumerSettings ConsumerSettings { get; set; }

        public Topics Topics { get; set; }
    }
}
