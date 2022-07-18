using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using YusX.Core.Extensions;

namespace YusX.Core.Providers.Validator
{
    public class NullObjectModelValidator : IObjectModelValidator
    {
        public void Validate(ActionContext context, ValidationStateDictionary validationState, string prefix, object model)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                context.ModelValidator(prefix, model);
            }
        }
    }
}
