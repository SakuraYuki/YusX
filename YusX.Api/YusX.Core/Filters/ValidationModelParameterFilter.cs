using System;
using System.Linq;
using YusX.Core.Extensions;
using YusX.Core.Providers.Validator;

namespace YusX.Core.Filters
{
    /// <summary>
    /// 方法模型参数验证过滤器
    /// </summary>
    public class ValidationModelParameterFilter : Attribute
    {
        /// <summary>
        /// 使用模型参数名称初始化
        /// </summary>
        /// <param name="modelName">模型参数名称</param>
        public ValidationModelParameterFilter(ModelParameterName modelName)
        {
            ModelParameterNames = modelName.GetModelParameters()?.Select(x => x.ToLower())?.ToArray();
        }

        /// <summary>
        /// 获取全部参数名称
        /// </summary>
        public string[] ModelParameterNames { get; }
    }
}
