using Common.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Common.Core.Interfaces
{
    /// <summary>
    /// Abstraction for queueing and commiting processing services within a transaction.
    /// Uses <see cref="Process"/> record storage for holding the process details.
    /// </summary>
    public interface IProcessQueue : ITransactionalQueue
    {
        /// <summary>
        /// Queue service with specified arguments, optional schedule time, and optional continuation from a previous process.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="arguments">Custom arguments for the service process.</param>
        /// <param name="scheduledDate">Optional date/time to schedule the process.</param>
        /// <param name="afterProcessGuid">Optional parent process identifier on which to queue the service only after the parent process succeeds.</param>
        /// <returns>Result object containing the <see cref="Process"/> and means to queue another service after this service.</returns>
        Task<ProcessQueueResult> EnqueueAsync<TService, TArgs>(TArgs arguments, DateTime? scheduledDate = null, Guid? afterProcessGuid = null)
            where TService : IProcessService<TArgs>
            where TArgs : ProcessArguments;

        /// <summary>
        /// Attempt to stop an assumed running process.
        /// </summary>
        /// <param name="process">Process to stop. Assumes process is currently executing, queued, or scheduled.</param>
        /// <returns>True if process was successfully stopped. False if process was not able to be stopped or was already stopped.</returns>
        bool TryStop(Process process);
    }
}
