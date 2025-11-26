using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Common.Core;
using Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// MVC controller/action filter to begin and commit or rollback a transaction around the action or actions under a controller.
    /// Uses <see cref="IUnitOfWork"/> and any <see cref="ITransactionalQueue"/> implemenations present on the registered service collection.
    /// The transactions will not be committed if an exception occurs or transaction is manually set to rollback 
    /// on <see cref="Controller.TempData"/> via <see cref="TempDataDictionaryExtensions.SetTransactionStatus(Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary, bool)"/>.
    /// Otherwise, transactions are committed using <see cref="IUnitOfWork.CommitTransaction"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CommandTransactionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Optional time in seconds to delay committing transaction values.
        /// </summary>
        public int Delay { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // get an optional logger
            context.HttpContext.TryGetService(out ILogger<CommandTransactionAttribute> logger);

            // try get IUnitOfWork which is semi-expected here but not required
            if (!context.HttpContext.TryGetService(out IUnitOfWork unitOfWork))
                logger?.LogWarning($"Type {typeof(IUnitOfWork).FullName} is not registered with the service collection for use with the {nameof(CommandTransactionAttribute)}.");

            // if valid response and unit of work is registered, begin a transaction on the unit of work
            if (context.HttpContext.Response.StatusCode != (int)HttpStatusCode.NotFound)
            {
                unitOfWork?.BeginTransaction();
            }

            // continue to next step
            var result = await next();

            // rollback or commit based on the result
            if (result.ShouldRollbackTransaction())
            {
                // NOTE: only IUnitOfWork requires the explicit rollback
                unitOfWork?.RollbackTransaction(result.Exception);
            }
            else
            {
                unitOfWork?.CommitTransaction();

                // if transactional queues are registered, commit those (rollback is not required)
                if (context.HttpContext.TryGetServices(out IEnumerable<ITransactionalQueue> transactionQueues))
                {
                    foreach (var queue in transactionQueues)
                    {
                        logger?.LogDebug($"Committing transactional queue {queue.GetType().FullName}...");

                        TimeSpan? delay = null;
                        if (Delay > 0)
                        {
                            delay = TimeSpan.FromSeconds(Delay);
                            logger?.LogDebug($"Committing queue using delay ({Delay} seconds) {delay}.");
                        }

                        queue.Commit(delay);
                    }
                } 
                else // not logging as warning as the ITransactionalQueues will be less common than IUnitOfWork
                    logger?.LogInformation($"Types {typeof(ITransactionalQueue).FullName} not found in service registration for use with the {nameof(CommandTransactionAttribute)} for committing transactions.");
            }
        }
    }
}
