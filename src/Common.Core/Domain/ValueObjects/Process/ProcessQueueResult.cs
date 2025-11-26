using Common.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Common.Core.Domain
{
    /// <summary>
    /// Process queuing result object containing the <see cref="Process"/> 
    /// that was queued and means to enqueue another process.
    /// </summary>
    public class ProcessQueueResult : ValueObject<ProcessQueueResult>
    {
        public ProcessQueueResult(Process process, IProcessQueue queue)
        {
            Process = process ?? throw new ArgumentNullException(nameof(process));
            Queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        /// <summary>
        /// Process information for the queued process service.
        /// </summary>
        public Process Process { get; }
        internal IProcessQueue Queue { get; }

        /// <summary>
        /// Queue another process service to be executed after the process of this result completes.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="arguments">Custom arguments for the service process.</param>
        /// <returns>Result object containing the <see cref="Process"/> and means to queue another service after this service.</returns>
        public Task<ProcessQueueResult> ThenEnqueueAsync<TService, TArgs>(TArgs arguments)
            where TService : IProcessService<TArgs>
            where TArgs : ProcessArguments
        {
            return Queue.EnqueueAsync<TService, TArgs>(arguments, afterProcessGuid: Process.Guid);
        }
    }
}
