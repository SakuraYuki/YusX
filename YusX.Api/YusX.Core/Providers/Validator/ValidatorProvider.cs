using Microsoft.Extensions.DependencyInjection;
using YusX.Core.Extensions;
using YusX.Entity.System;

namespace YusX.Core.Providers.Validator
{
    /// <summary>
    /// 验证器提供者
    /// </summary>
    public static class ValidatorProvider
    {
        /// <summary>
        /// 添加方法的模型参数验证器
        /// </summary>
        /// <param name="services">服务</param>
        /// <returns></returns>
        public static IServiceCollection AddMethodModelParameterValidator(this IServiceCollection services)
        {
            // 登陆方法校验参数,只验证密码与用户名
            ModelParameterName.Login.Add<LoginInfo>(x => new { x.Password, x.Username, x.VerificationCode, x.UUID });

            // 只验证LoginInfo的密码字段必填
            ModelParameterName.LoginOnlyPassWord.Add<LoginInfo>(x => new { x.Password });

            return services;
        }

        /// <summary>
        /// 添加方法的普通参数验证器
        /// </summary>
        /// <param name="services">服务</param>
        /// <returns></returns>
        public static IServiceCollection AddMethodGeneralParameterValidator(this IServiceCollection services)
        {
            //配置用户名最多30个字符
            GeneralParameterName.Username.Add("用户名", 30);

            //方法参数名为newPwd，直接在方法加上[ObjectGeneralValidatorFilter(ValidatorGeneral.NewPwd)]进行参数验证
            //如果newPwd为空会提示：新密码不能为空
            //6,50代表newPwd参数最少6个字符，最多50个符
            //其他需要验证的参数同样配置即可
            GeneralParameterName.NewPwd.Add("新密码", 6, 50);

            //如果OldPwd为空会提示：旧密码不能为空
            GeneralParameterName.OldPwd.Add("旧密码");

            //校验手机号码格式
            GeneralParameterName.PhoneNo.Add("手机号码", (object value) =>
            {
                var validatorResult = new ObjectValidatorResult(true);
                if (!value.ToString().IsPhoneNo())
                {
                    validatorResult = validatorResult.Error("请输入正确的手机号码");
                }
                return validatorResult;
            });

            //测试验证字符长度为6-10
            GeneralParameterName.Local.Add("所在地", 6, 10);

            //测试验证数字范围
            GeneralParameterName.Qty.Add("存货量", ParameterType.Int, 200, 500);

            return services;
        }
    }

    /// <summary>
    /// 模型参数名称
    /// </summary>
    public enum ModelParameterName
    {
        Login,
        LoginOnlyPassWord,
    }

    /// <summary>
    /// 普通参数名称
    /// </summary>
    /// <remarks>
    /// 枚举的名字必须与参数名字一致，不区分大小写
    /// </remarks>
    public enum GeneralParameterName
    {
        Username,
        OldPwd,
        NewPwd,
        PhoneNo,
        Local,
        Qty,
    }
}
