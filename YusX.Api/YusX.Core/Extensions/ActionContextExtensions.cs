using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;
using YusX.Core.Constracts;
using YusX.Core.Filters;
using YusX.Core.Providers.Validator;
using YusX.Core.Utilities;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 动作上下文扩展
    /// </summary>
    public static class ActionContextExtensions
    {
        /// <summary>
        /// 设置表示未授权的响应数据
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <param name="message">响应消息</param>
        public static void SetUnauthorizedResult(this ActionExecutingContext context, string message)
        {
            context.Result = new ContentResult()
            {
                Content = new WebResponseContent
                {
                    Status = false,
                    Message = message
                }.Serialize(),
                ContentType = ApplicationContentType.JSON,
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
        }

        /// <summary>
        /// 设置错误消息的结果
        /// </summary>
        /// <remarks>
        /// 设置 JSON 格式的返回内容：{ "Status": false, "Message": "错误消息" }
        /// </remarks>
        /// <param name="context">动作上下文</param>
        /// <param name="message">错误消息</param>
        public static void SetErrorResult(this ActionExecutingContext context, string message)
        {
            context.Result = new ContentResult()
            {
                Content = new WebResponseContent
                {
                    Status = false,
                    Message = message
                }.Serialize(),
                ContentType = ApplicationContentType.JSON,
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
        }

        /// <summary>
        /// 当前动作上下文是否添加了模型验证过滤器
        /// </summary>
        /// <param name="context">动作上下文</param>
        /// <returns></returns>
        public static bool ExistsModelParameterValidator(this ActionExecutingContext context)
            => context.ActionDescriptor.EndpointMetadata.Any(item => item is ValidationModelParameterFilter);

        /// <summary>
        /// 获取当前动作上下文的模型参数的全部参数名称
        /// </summary>
        /// <param name="context">动作上下文</param>
        /// <returns></returns>
        public static string[] GetModelParameterNames(this ActionContext context)
            => (context.ActionDescriptor.EndpointMetadata.FirstOrDefault(f => f is ValidationModelParameterFilter) as ValidationModelParameterFilter)?.ModelParameterNames;

        /// <summary>
        /// 获取当前动作上下文的全部普通参数选项
        /// </summary>
        /// <param name="context">动作上下文</param>
        /// <returns></returns>
        public static GeneralParameterOptions[] GetGeneralParameterOptions(this ActionContext context)
            => (context.ActionDescriptor.EndpointMetadata.FirstOrDefault(f => f is ValidationGeneralParameterFilter) as ValidationGeneralParameterFilter)?.ParameterOptions;

        /// <summary>
        /// 验证当前动作上下文的全部方法参数
        /// </summary>
        /// <param name="context">动作上下文</param>
        public static void ValidationParameter(this ActionExecutingContext context)
        {
            // 检验普通参数
            context.ValidationGeneralParameter();

            // 是否存在已配置模型验证器
            if (!context.ExistsModelParameterValidator())
            {
                return;
            }

            // 判断当前模型是否已验证（验证状态默认为true，如果为false代表已验证，并且验证错误）
            var validatorState = context.GetModelValidatorState();
            if (!validatorState.Status)
            {
                context.SetErrorResult(validatorState.Message);
                return;
            }

            // 是否存在模型参数
            if (!validatorState.HasModelParameter)
            {
                context.SetErrorResult($"未提交模型参数");
                return;
            }
        }

        /// <summary>
        /// 校验当前动作上下文的方法普通参数
        /// </summary>
        /// <param name="context">动作上下文</param>
        public static void ValidationGeneralParameter(this ActionExecutingContext context)
        {
            var parameterOptions = context.GetGeneralParameterOptions();

            if (parameterOptions == null)
            {
                return;
            }

            foreach (var options in parameterOptions)
            {
                // 已处理过
                if (context.Result != null)
                {
                    return;
                }

                if (!context.HttpContext.Request.Query.TryGetValue(options.Name, out var paramVal))
                {
                    context.SetErrorResult($"请提交参数[{options.NameCn}]");
                    return;
                }

                if (string.IsNullOrWhiteSpace(paramVal))
                {
                    context.SetErrorResult($"参数[{options.NameCn}]不能为空");
                    return;
                }

                // 优先使用自定义验证器
                if (options.CustomValidator != null)
                {
                    var validatorResult = options.CustomValidator(paramVal);
                    if (!validatorResult.Status)
                    {
                        context.SetErrorResult(validatorResult.Message);
                        return;
                    }
                    continue;
                }

                context.ValidationValue(options, paramVal);
            }
        }

        /// <summary>
        /// 验证方法普通参数值
        /// </summary>
        /// <param name="actionContext">动作上下文</param>
        /// <param name="options">普通参数验证选项</param>
        /// <param name="paramVal">参数值</param>
        public static void ValidationValue(this ActionExecutingContext actionContext, GeneralParameterOptions options, object paramVal)
        {
            if (paramVal == null)
            {
                actionContext.SetErrorResult($"请提交参数{options.NameCn}");
                return;
            }

            if (options.Min == null && options.Max == null)
            {
                return;
            }

            switch (options.ParameterType)
            {
                case ParameterType.Int:
                    #region Int 整数

                    if (!int.TryParse(paramVal.ToString(), out var intVal))
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]必须是整数");
                        break;
                    }
                    if (options.Min != null && intVal < options.Min)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不能小于[{options.Min}]");
                        break;
                    }
                    if (options.Max != null && intVal > options.Max)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不能大于[{options.Max}]");
                    }

                    #endregion Int 整数
                    break;
                case ParameterType.Long:
                    #region Long 长整数

                    if (!long.TryParse(paramVal.ToString(), out var longVal))
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]必须是长整数");
                        break;
                    }
                    if (options.Min != null && longVal < options.Min)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不能小于[{options.Min}]");
                        break;
                    }
                    if (options.Max != null && longVal > options.Max)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不能大于[{options.Max}]");
                    }

                    #endregion Long 长整数
                    break;
                case ParameterType.Decimal:
                    #region Decimal 数字

                    if (!decimal.TryParse(paramVal.ToString(), out var decimalVal))
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不是数字");
                        break;
                    }
                    if (options.Min != null && decimalVal < options.Min)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不能小于[{options.Min}]");
                        break;
                    }
                    if (options.Max != null && decimalVal > options.Max)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不能大于[{options.Max}]");
                    }

                    #endregion Decimal 数字
                    break;
                case ParameterType.Byte:
                    #region Byte 字节

                    if (!byte.TryParse(paramVal.ToString(), out var byteVal))
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不是字节");
                        break;
                    }
                    if (options.Min != null && byteVal < options.Min)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不能小于[{options.Min}]");
                        break;
                    }
                    if (options.Max != null && byteVal > options.Max)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不能大于[{options.Max}]");
                    }

                    #endregion Byte 字节
                    break;
                case ParameterType.String:
                    #region String 字符串

                    string value = paramVal.ToString();
                    if (options.Min != null && value.Length < options.Min)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]至少[{options.Min}]个字符");
                    }
                    if (options.Max != null && value.Length > options.Max)
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]最多[{options.Max}]个字符");
                    }

                    #endregion String 字符串
                    break;
                case ParameterType.DateTime:
                    #region DateTime 日期时间

                    if (!DateTime.TryParse(paramVal.ToString(), out var dateTimeVal))
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不是日期时间");
                    }
                    var dateTimeJsVal = dateTimeVal.ToJsTimestamp();
                    if (options.Min != null && dateTimeJsVal < options.Min)
                    {
                        var minDateTime = ((long)options.Min).JsToDateTime();
                        actionContext.SetErrorResult($"[{options.NameCn}]不能早于[{minDateTime:yyyy-MM-dd HH:mm:ss}]");
                        break;
                    }
                    if (options.Max != null && dateTimeJsVal > options.Max)
                    {
                        var maxDateTime = ((long)options.Max).JsToDateTime();
                        actionContext.SetErrorResult($"[{options.NameCn}]不能晚于[{maxDateTime:yyyy-MM-dd HH:mm:ss}]");
                    }

                    #endregion DateTime 日期时间
                    break;
                case ParameterType.Bool:
                    #region Bool 布尔

                    if (!bool.TryParse(paramVal.ToString(), out _))
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不是布尔类型");
                    }

                    #endregion Bool 布尔
                    break;
                case ParameterType.Guid:
                    #region Guid 全局唯一标识符

                    if (!Guid.TryParse(paramVal.ToString(), out _))
                    {
                        actionContext.SetErrorResult($"[{options.NameCn}]不是Guid类型");
                    }

                    #endregion Guid 全局唯一标识符
                    break;
                default:
                    break;
            }
        }
    }
}
