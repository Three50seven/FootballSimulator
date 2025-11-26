using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.AspNetCore.Mvc
{
    public static class ActionExecutedContextExtensions
    {
        public static bool ShouldRollbackTransaction(this ActionExecutedContext context)
        {
            // rollback on exceptions
            if (context.Exception != null)
                return true;

            // cast as Controller
            var controller = (Controller)context.Controller;
            if (controller == null)
                return true;

            // if TempData for this requests designates flag to rollback(defaulted to false if not set)
            return controller.TempData.ShouldRollbackTransaction();
        }
    }
}
