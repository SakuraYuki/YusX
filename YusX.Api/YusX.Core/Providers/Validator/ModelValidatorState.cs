namespace YusX.Core.Providers.Validator
{
    /// <summary>
    /// 模型验证器状态
    /// </summary>
    public class ModelValidatorState
    {
        /// <summary>
        /// 空构造器，需手动赋值
        /// </summary>
        public ModelValidatorState()
        {
        }

        /// <summary>
        /// 验证状态
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// 是否存在模型参数
        /// </summary>
        public bool HasModelParameter { get; set; }

        /// <summary>
        /// 状态代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string Message { get; set; }
    }
}
