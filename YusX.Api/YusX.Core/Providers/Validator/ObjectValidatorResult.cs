namespace YusX.Core.Providers.Validator
{
    /// <summary>
    /// 对象验证器结果
    /// </summary>
    public class ObjectValidatorResult
    {
        /// <summary>
        /// 空构造器，需手动赋值
        /// </summary>
        public ObjectValidatorResult()
        {

        }

        /// <summary>
        /// 只用指定状态进行构造
        /// </summary>
        /// <param name="status">结果状态</param>
        public ObjectValidatorResult(bool status)
        {
            Status = status;
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 验证附加消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 标记为验证通过，并设置附加消息
        /// </summary>
        /// <param name="message">验证附加消息</param>
        /// <returns></returns>
        public ObjectValidatorResult OK(string message)
        {
            Status = true;
            Message = message;
            return this;
        }

        /// <summary>
        /// 标记为验证不通过，并设置附加消息
        /// </summary>
        /// <param name="message">验证附加消息</param>
        /// <returns></returns>
        public ObjectValidatorResult Error(string message)
        {
            Status = false;
            Message = message;
            return this;
        }
    }
}
