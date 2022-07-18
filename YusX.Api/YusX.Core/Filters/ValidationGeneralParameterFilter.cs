using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using YusX.Core.Extensions;
using YusX.Core.Providers.Validator;

namespace YusX.Core.Filters
{
    /// <summary>
    /// 方法普通参数验证过滤器
    /// </summary>
    public class ValidationGeneralParameterFilter : Attribute, IFilterMetadata
    {
        /// <summary>
        /// 使用多个普通参数名称初始化参数验证过滤器
        /// </summary>
        /// <param name="names">普通参数名称</param>
        public ValidationGeneralParameterFilter([NotNull] params GeneralParameterName[] names)
        {
            ParameterOptions = names.GetGeneralOption().ToArray();
        }

        /// <summary>
        /// 获取全部参数选项
        /// </summary>
        public GeneralParameterOptions[] ParameterOptions { get; }
    }
}
