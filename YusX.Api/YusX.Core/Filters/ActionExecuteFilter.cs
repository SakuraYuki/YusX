using Microsoft.AspNetCore.Mvc.Filters;
using YusX.Core.Validator;

namespace YusX.Core.Filters
{
    public class ActionExecuteFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 验证方法参数
            context.ActionParamsValidator();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}