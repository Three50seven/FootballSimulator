using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// MVC controller/action filter designed to check for any active <see cref="ITransactionalQueue"/> queues
    /// and commit all items found on each queue calling <see cref="ITransactionalQueue.Commit"/>.
    /// The queues will not be committed if an exception occurs or transaction is manually set to rollback on <see cref="Controller.TempData"/>. 
    /// </summary>
    [Obsolete("Use CommandTransactionAttribute instead. This will be removed in a future version.")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class TransactionalQueueAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!ShouldRollbackTransaction(context) && TryGetQueues(context.HttpContext, out IEnumerable<ITransactionalQueue> queues))
            {
                foreach (var queue in queues)
                {
                    queue.Commit();
                }
            }
        }

        /// <summary>
        /// Attempt to get list of <see cref="ITransactionalQueue"/> from the service collection.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="queues"></param>
        /// <returns>True if one or more queues were found. False if none were found.</returns>
        protected virtual bool TryGetQueues(HttpContext httpContext, out IEnumerable<ITransactionalQueue> queues)
        {
            queues = httpContext.RequestServices.GetServices<ITransactionalQueue>();
            return queues != null && queues.Any();
        }

        /// <summary>
        /// Based on the status of the action context on whether or not the transactional queues should be committed.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
