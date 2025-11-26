using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Common.Core;
using System;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Serializes an invalid ModelState on OnActionExecuted when the result is redirection.
    /// Intended to be used on form POST actions where invalid requests are redirected to form GET action decorated with <see cref="ImportModelStateAttribute"/>.
    /// </summary>
    public class ExportInvalidModelStateAttribute : ModelStateTransferringAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // only export when ModelState is not valid and if redirecting
            if (filterContext.ModelState != null
                && !filterContext.ModelState.IsValid 
                && (filterContext.Result is RedirectResult 
                || filterContext.Result is RedirectToRouteResult 
                || filterContext.Result is RedirectToActionResult))
            {
                if (filterContext.Controller is Controller controller)
                {
                    var serializer = (ISerializer)filterContext.HttpContext.RequestServices.GetService(typeof(ISerializer)) ?? throw new InvalidOperationException($"Required service implementation not found for {typeof(ISerializer).FullName}.");
                    controller.TempData[Key] = serializer.SerializeModelState(filterContext.ModelState);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
