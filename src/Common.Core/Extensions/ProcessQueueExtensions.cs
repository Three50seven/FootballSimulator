using Common.Core.Domain;
using Common.Core.Interfaces;
using Common.Core.Validation;
using System;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class ProcessQueueExtensions
    {
        /// <summary>
        /// Schedule a process service for a future date/time.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TParams"></typeparam>
        /// <param name="queue"></param>
        /// <param name="arguments">Custom arguments for the service process.</param>
        /// <param name="date">Required future date/time on which to execute the service.</param>
        /// <returns>Result object containing the <see cref="Process"/> and means to queue another service after this service.</returns>
        public static Task<ProcessQueueResult> ScheduleAsync<TService, TParams>(
           this IProcessQueue queue,
           TParams arguments,
           DateTime date)
           where TService : IProcessService<TParams>
           where TParams : ProcessArguments
        {
            Guard.IsNotNull(queue, nameof(queue));

            return queue.EnqueueAsync<TService, TParams>(arguments, scheduledDate: date);
        }

        /// <summary>
        /// Queue a process service after a specified parent process has completed successfully.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TParams"></typeparam>
        /// <param name="queue"></param>
        /// <param name="arguments">Custom arguments for the service process.</param>
        /// <param name="afterProcessGuid">Required parent process identifier on which to queue the service only after the parent process succeeds</param>
        /// <returns>Result object containing the <see cref="Process"/> and means to queue another service after this service.</returns>
        public static Task<ProcessQueueResult> EnqueueAfterAsync<TService, TParams>(
            this IProcessQueue queue,
            TParams arguments,
            Guid afterProcessGuid)
            where TService : IProcessService<TParams>
            where TParams : ProcessArguments
        {
            Guard.IsNotNull(queue, nameof(queue));
            Guard.IsNotDefaultValueType(afterProcessGuid, nameof(afterProcessGuid));

            return queue.EnqueueAsync<TService, TParams>(arguments, afterProcessGuid: afterProcessGuid);
        }
    }
}
