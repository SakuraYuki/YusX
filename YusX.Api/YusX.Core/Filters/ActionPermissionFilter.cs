using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using YusX.Core.Configuration;
using YusX.Core.Enums;
using YusX.Core.ManageUser;
using YusX.Core.Services;
using YusX.Core.Utilities;
using YusX.Entity.Attributes;

namespace YusX.Core.Filters
{
    /// <summary>
    /// 1、控制器或controller设置了AllowAnonymousAttribute直接返回
    /// 2、TableName、TableAction 同时为null，SysController为false的，只判断是否登陆
    /// 3、TableName、TableAction 都不null根据表名与action判断是否有权限
    /// 4、SysController为true，通过httpcontext获取表名与action判断是否有权限
    /// 5、Roles对指定角色验证
    /// </summary>
    public class ActionPermissionFilter : IAsyncActionFilter
    {
        public ActionPermissionFilter(ActionPermissionRequirement actionPermissionRequirement, UserContext userContext)
        {
            this.ResponseContent = new WebResponseContent();
            this.actionPermission = actionPermissionRequirement;
            UserContext = userContext;
        }

        private WebResponseContent ResponseContent { get; set; }

        private readonly ActionPermissionRequirement actionPermission;

        private UserContext UserContext { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (OnActionExecutionPermission(context).Status)
            {
                await next();
                return;
            }
            FilterResponse.SetUnauthorizedResult(context, ResponseContent?.Message);
        }

        private WebResponseContent OnActionExecutionPermission(ActionExecutingContext context)
        {
            //!context.Filters.Any(item => item is IFixedTokenFilter))固定token的是否验证权限
            //if ((context.Filters.Any(item => item is IAllowAnonymousFilter)
            //    && !context.Filters.Any(item => item is IFixedTokenFilter))
            //    || UserContext.Current.IsSuperAdmin
            //    )
            if (context.Filters.Any(item => item is IAllowAnonymousFilter)
                || UserContext.Current.IsSuperAdmin)
                return ResponseContent.OK();

            //演示环境除了admin帐号，其他帐号都不能增删改等操作
            if (!UserContext.IsSuperAdmin && AppSetting.GlobalFilter.Enable
                && AppSetting.GlobalFilter.Actions.Contains(((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName))
            {
                return ResponseContent.Error(AppSetting.GlobalFilter.Message);
            }

            //如果没有指定表的权限，则默认为代码生成的控制器，优先获取PermissionTableAttribute指定的表，如果没有数据则使用当前控制器的名作为表名权限
            if (actionPermission.SysController)
            {
                object[] permissionArray = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor)?.ControllerTypeInfo.GetCustomAttributes(typeof(PermissionTableAttribute), false);
                if (permissionArray == null || permissionArray.Length == 0)
                {
                    actionPermission.TableName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
                }
                else
                {
                    actionPermission.TableName = (permissionArray[0] as PermissionTableAttribute).Name;
                }
                if (string.IsNullOrEmpty(actionPermission.TableName))
                {
                    //responseType = ResponseType.ParametersLack;
                    return ResponseContent.Error(ResponseType.ParametersLack);
                }
            }

            //如果没有给定权限，不需要判断
            if (string.IsNullOrEmpty(actionPermission.TableName)
                && string.IsNullOrEmpty(actionPermission.TableAction)
                && (actionPermission.RoleIds == null || actionPermission.RoleIds.Length == 0))
            {
                return ResponseContent.OK();
            }

            //是否限制的角色ID称才能访问
            //权限判断角色ID,
            if (actionPermission.RoleIds != null && actionPermission.RoleIds.Length > 0)
            {
                if (actionPermission.RoleIds.Contains(UserContext.UserInfo.Role_Id)) return ResponseContent.OK();
                //如果角色ID没有权限。并且也没控制器权限
                if (string.IsNullOrEmpty(actionPermission.TableAction))
                {
                    return ResponseContent.Error(ResponseType.NoRolePermissions);
                }
            }
            //2020.05.05移除x.TableName.ToLower()转换,获取权限时已经转换成为小写
            var actionAuth = UserContext.GetPermissions(x => x.TableName == actionPermission.TableName.ToLower())?.UserAuthArr;

            if (actionAuth == null
                 || actionAuth.Count() == 0
                 || !actionAuth.Contains(actionPermission.TableAction))
            {
                ApiLogger.Info(ApiLogType.Authorzie, $"没有权限操作," +
                    $"用户ID{UserContext.UserId}:{UserContext.UserTrueName}," +
                    $"角色ID:{UserContext.RoleId}:{UserContext.UserInfo.RoleName}," +
                    $"操作权限{actionPermission.TableName}:{actionPermission.TableAction}");
                return ResponseContent.Error(ResponseType.NoPermissions);
            }
            return ResponseContent.OK();
        }
    }
}
