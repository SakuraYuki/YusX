using System;

namespace YusX.Core.Providers.Validator
{
    /// <summary>
    /// 普通参数选项
    /// </summary>
    public class GeneralParameterOptions
    {
        /// <summary>
        /// 构造字符串类型的普通参数选项
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        public GeneralParameterOptions(GeneralParameterName paramName, string nameCn)
            : this(paramName, nameCn, ParameterType.String)
        {
        }

        /// <summary>
        /// 构造指定类型的普通参数选项
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="type">参数类型</param>
        public GeneralParameterOptions(GeneralParameterName paramName, string nameCn, ParameterType type)
            : this(paramName, nameCn, type, null, null)
        {
        }

        /// <summary>
        /// 构造字符串类型的普通类型参数，并指定字符串的最小长度与最大长度
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="min">字符串长度最小值</param>
        /// <param name="max">字符串长度最大值</param>
        public GeneralParameterOptions(GeneralParameterName paramName, string nameCn, decimal? min, decimal? max)
            : this(paramName, nameCn, ParameterType.String, min, max)
        {
        }

        /// <summary>
        /// 构造指定类型的普通参数选项，并指定参数的最小值与最大值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="min">参数最小值</param>
        /// <param name="max">参数最大值</param>
        public GeneralParameterOptions(GeneralParameterName paramName, string nameCn, ParameterType type, decimal? min, decimal? max)
        {
            Name = paramName.ToString().ToLower();
            NameCn = nameCn;
            ParameterType = type;
            Min = min;
            Max = max;
        }

        /// <summary>
        /// 使用自定义验证器构造指定类型的普通参数选项
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="customValidator"></param>
        public GeneralParameterOptions(GeneralParameterName paramName, string nameCn, Func<object, ObjectValidatorResult> customValidator)
        {
            Name = paramName.ToString().ToLower();
            NameCn = nameCn;
            CustomValidator = customValidator;
        }

        /// <summary>
        /// 方法中的参数名称，忽略大小写
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 中文名字，参数校验错误时的提示文字
        /// </summary>
        public string NameCn { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterType ParameterType { get; set; }

        /// <summary>
        /// 参数最小值，数值类为最小值，字符串为最小长度
        /// </summary>
        public decimal? Min { get; set; }

        /// <summary>
        /// 参数最大值，数值类为最大值，字符串为最大长度
        /// </summary>
        public decimal? Max { get; set; }

        /// <summary>
        /// 自定义验证器
        /// </summary>
        public Func<object, ObjectValidatorResult> CustomValidator { get; set; }
    }

    /// <summary>
    /// 参数类型
    /// </summary>
    public enum ParameterType
    {
        Int,
        Long,
        Decimal,
        Byte,
        String,
        DateTime,
        Bool,
        Guid,
    }
}
