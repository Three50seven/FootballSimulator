using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Common.Core.Validation;
using System.Net;

namespace Common.AspNetCore.Mvc
{
    public static class ActionExecutingContextExtensions
    {
        public static void SetFailedResult(this ActionExecutingContext context, ValidationRule rule)
        {
            SetFailedResult(context, new BrokenRulesList(rule));
        }

        public static void SetFailedResult(this ActionExecutingContext context, BrokenRulesList brokenRules)
        {
            if (context.HttpContext.Request.IsAjax())
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(brokenRules);
            }
            else
            {
                if (context.Controller is not Controller controller)
                    throw new InvalidCastException($"Controller on {nameof(ActionExecutedContext)} not found or is not of type {typeof(Controller).FullName}.");

                controller.ModelState.AddValidationRuleErrors(brokenRules);
                context.Result = new ViewResult()
                {
                    ViewName = "ValidationErrors",
                    ViewData = new ViewDataDictionary(controller.ViewData)
                };
            }
        }
    }
}
