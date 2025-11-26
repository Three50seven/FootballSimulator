using Common.Core.Domain;
using System;

namespace Common.Core.Interfaces
{
    /// <summary>
    /// Abstract service to schedule recurring Process service tasks that will excecute a service under <see cref="Process"/> records.
    /// </summary>
    public interface IProcessRecurringScheduler
    {
        /// <summary>
        /// Add or update Process service task recurring schedule under a given unique id and using Process record for tracking service execution stats.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="id">Unique identifier for this process service recurring task.</param>
        /// <param name="arguments">Custom arguments needed to perform the process service action.</param>
        /// <param name="cronExpression">Cron expression that represents the recurring schedule.</param>
        /// <param name="timeZone">Optional time zone to use for the cron expression. Should default to Utc.</param>
        void AddOrUpdate<TService, TArgs>(string id, TArgs arguments, string cronExpression, TimeZoneInfo timeZone = null)
            where TService : IProcessService<TArgs>
            where TArgs : ProcessArguments;

        /// <summary>
        /// Execute an established Process service recurring task immediately given its unique id.
        /// </summary>
        /// <param name="id"></param>
        void Trigger(string id);

        /// <summary>
        /// Remove an established Process service recurring task from the schedule given its unique id.
        /// </summary>
        /// <param name="id"></param>
        void Remove(string id);
    }
}
