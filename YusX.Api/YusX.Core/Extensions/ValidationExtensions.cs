using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using YusX.Core.Providers.Validator;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 验证扩展
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// 模型参数校验配置
        /// </summary>
        public static Dictionary<string, string[]> ValidatorCollection { get; } = new Dictionary<string, string[]>();

        /// <summary>
        /// 普通参数校验配置
        /// </summary>
        public static Dictionary<string, GeneralParameterOptions> GeneralValidatorOptions { get; } = new Dictionary<string, GeneralParameterOptions>();

        /// <summary>
        /// 检验参数必须为字符串
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        public static void Add(this GeneralParameterName paramName, string nameCn)
        {
            paramName.Add(nameCn, ParameterType.String, null, null);
        }

        /// <summary>
        /// 校验参数必须为字符串，且指定其长度的最大值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="max">字符串长度最大值</param>
        public static void Add(this GeneralParameterName paramName, string nameCn, int? max)
        {
            paramName.Add(nameCn, ParameterType.String, null, max);
        }

        /// <summary>
        /// 校验参数必须为字符串，且指定其长度的最小值与最大值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="min">字符串长度最小值</param>
        /// <param name="max">字符串长度最大值</param>
        public static void Add(this GeneralParameterName paramName, string nameCn, int? min, int? max)
        {
            paramName.Add(nameCn, ParameterType.String, min, max);
        }

        /// <summary>
        /// 校验参数必须为指定类型
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="type">参数类型</param>
        public static void Add(this GeneralParameterName paramName, string nameCn, ParameterType type)
        {
            paramName.Add(nameCn, type, null, null);
        }

        /// <summary>
        /// 校验参数必须为指定类型，且指定其最大值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="max">参数最大值</param>
        public static void Add(this GeneralParameterName paramName, string nameCn, ParameterType type, int? max)
        {
            paramName.Add(nameCn, type, null, max);
        }

        /// <summary>
        /// 校验参数必须为指定类型，且指定其最小值与最大值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="min">参数最小值</param>
        /// <param name="max">参数最大值</param>
        public static void Add(this GeneralParameterName paramName, string nameCn, ParameterType type, int? min = null, int? max = null)
        {
            var options = new GeneralParameterOptions(paramName, nameCn, type, min, max);
            if (!GeneralValidatorOptions.TryAdd(paramName.ToString().ToLower(), options))
            {
                throw new Exception($"键{paramName}参数配置已经注入过了");
            }
        }

        /// <summary>
        /// 检验参数是否符合指定的检验规则
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="nameCn">校验错误时显示的提示名称</param>
        /// <param name="customValidator">自定义的校验规则表达式</param>
        /// <exception cref="ArgumentException"><paramref name="paramName"/>已配置过，无法重复配置</exception>
        public static void Add(this GeneralParameterName paramName, string nameCn, Func<object, ObjectValidatorResult> customValidator)
        {
            var options = new GeneralParameterOptions(paramName, nameCn, customValidator);
            if (!GeneralValidatorOptions.TryAdd(paramName.ToString().ToLower(), options))
            {
                throw new ArgumentException($"参数名{paramName}无法重复配置");
            }
        }

        /// <summary>
        /// 检验参数是否符合指定的检验规则
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="fieldExpression">需要验证字段的表达式</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Add<T>(this ModelParameterName paramName, Expression<Func<T, object>> fieldExpression = null)
        {
            var modelfieldNames = fieldExpression == null
                ? typeof(T).GetGenericProperties().Select(x => x.Name).ToArray()
                : fieldExpression.GetExpressionToArray();

            if (!ValidatorCollection.TryAdd(paramName.ToString().ToLower(), modelfieldNames))
            {
                throw new ArgumentException($"参数名{paramName}无法重复配置");
            }
        }

        /// <summary>
        /// 获取方法上绑定的model校验字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string[] GetModelParameters(this ModelParameterName name)
        {
            return name.ToString().GetModelParameters();
        }

        /// <summary>
        /// 获取方法上绑定的model校验字段
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public static string[] GetModelParameters(this string modelName)
        {
            if (!ValidatorCollection.TryGetValue(modelName.ToLower(), out string[] values))
            {
                Console.WriteLine($"未注册{modelName}参数的表达式");
                throw new Exception($"未注册{modelName}参数的表达式");
            }
            return values;
        }

        /// <summary>
        /// 获取方法上绑定校验字段的配置信息
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static IEnumerable<GeneralParameterOptions> GetGeneralOption(this GeneralParameterName[] general)
        {
            return general.Select(x => x.ToString()).ToArray().GetGeneralOptions();
        }

        /// <summary>
        /// 获取方法上绑定校验字段的配置信息
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static IEnumerable<GeneralParameterOptions> GetGeneralOptions(this string[] generalName)
        {
            foreach (string item in generalName)
            {
                if (!GeneralValidatorOptions.TryGetValue(item.ToLower(), out GeneralParameterOptions options))
                {
                    throw new Exception($"未注册{generalName}参数的配置");
                }
                yield return options;
            }
        }

        /// <summary>
        /// model校验
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static void ModelValidator(this ActionContext context, string prefix, object model)
        {
            var paramNames = context.GetModelParameterNames();

            if (paramNames == null)
            {
                return;
            }

            if (model == null)
            {
                context.ErrorResult("没有获取到参数");
                return;
            }
            //model==list未判断
            var properties = model.GetType().GetProperties().Where(x => paramNames.Contains(x.Name.ToLower())).ToArray();
            foreach (var item in properties)
            {
                if (!item.ValidationRquiredValueForDbType(item.GetValue(model), out string message))
                {
                    context.ErrorResult(message);
                    return;
                }
            }

            var validatorState = context.HttpContext.GetService<ModelValidatorState>();
            validatorState.HasModelParameter = true;
        }

        /// <summary>
        ///参数验证没有通过的直接返回校验结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        public static void ErrorResult(this ActionContext context, string message)
        {
            var validatorState = context.GetModelValidatorState();
            if (!validatorState.Status)
            {
                return;
            }
            validatorState.Status = false;
            validatorState.Message = message;
        }

        /// <summary>
        /// 获取当前上下文中的模型参数验证状态对象
        /// </summary>
        /// <param name="context">动作上下文</param>
        /// <returns></returns>
        public static ModelValidatorState GetModelValidatorState(this ActionContext context)
            => context.HttpContext.GetService<ModelValidatorState>();
    }
}
