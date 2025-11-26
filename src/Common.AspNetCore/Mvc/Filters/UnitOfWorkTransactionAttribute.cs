using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Common.Core;
using System;
using System.Net;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Wraps action in a transaction based on <see cref="IUnitOfWork"/>.
    /// Transaction will commit at the end of the action and rollback if an exception occurs.
    /// Typically this is good when committing multiple database operations and should be used 
    /// on any actions that commit to the database so <see cref="IUnitOfWork.Save"/> is invoked.
    /// Rollback can occur manually by calling <see cref="BaseController.SetTransactionToRollback"/> 
    /// or <see cref="TempDataDictionaryExtensions.SetTransactionStatus(Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary, bool)"/>.
    /// </summary>
    [Obsolete("Use CommandTransactionAttribute instead. This will be removed in a future version.")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class UnitOfWorkTransactionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var unitOfWork = GetUnitOfWork(context.HttpContext);
            if (ShouldOpenTransaction(context, unitOfWork))
                unitOfWork.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var unitOfWork = GetUnitOfWork(context.HttpContext);
            if (ShouldRollbackTransaction(context))
                unitOfWork.RollbackTransaction(context.Exception);
            else
                unitOfWork.CommitTransaction();
        }

        protected virtual IUnitOfWork GetUnitOfWork(HttpContext httpContext)
        {
            var unitOfWork = httpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            if (unitOfWork == null)
                throw new NullReferenceException(nameof(unitOfWork));
            return unitOfWork;
        }

        protected virtual bool ShouldOpenTransaction(ActionExecutingContext context, IUnitOfWork unitOfWork)
        {
            return unitOfWork != null
                && context.HttpContext.Response.StatusCode != (int)HttpStatusCode.NotFound;
        }

        protected virtual bool ShouldRollbackTransaction(ActionExecutedContext context)
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
