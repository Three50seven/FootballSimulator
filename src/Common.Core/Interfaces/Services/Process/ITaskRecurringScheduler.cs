using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Core.Interfaces
{
    /// <summary>
    /// Abstract service to schedule, trigger, and remove recurring generic background tasks and services.
    /// </summary>
    public interface ITaskRecurringScheduler
    {
        /// <summary>
        /// Add or update a generic action task recurring schedule under given unique id.
        /// </summary>
        /// <param name="id">Unique identifier for this task.</param>
        /// <param name="expression">Task to execute.</param>
        /// <param name="cronExpression">Cron expression that represents the recurring schedule.</param>
        /// <param name="queue">Optional queue to run the task.</param>
        /// <param name="timeZone">Optional time zone to use for the cron expression. Should default to Utc.</param>
        void AddOrUpdate(
            string id, 
            Expression<Action> expression, 
            string cronExpression, 
            string queue = null,
            TimeZoneInfo timeZone = null);

        /// <summary>
        /// Add or update a generic action task (function task) recurring schedule under given unique id.
        /// </summary>
        /// <param name="id">Unique identifier for this task.</param>
        /// <param name="expression">Task to execute.</param>
        /// <param name="cronExpression">Cron expression that represents the recurring schedule.</param>
        /// <param name="queue">Optional queue to run the task.</param>
        /// <param name="timeZone">Optional time zone to use for the cron expression. Should default to Utc.</param>
        void AddOrUpdate(
            string id, 
            Expression<Func<Task>> expression, 
            string cronExpression, 
            string queue = null,
            TimeZoneInfo timeZone = null);

        /// <summary>
        /// Add or update service task recurring schedule under a given unique id.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="id">Unique identifier for this task.</param>
        /// <param name="expression">Task to execute.</param>
        /// <param name="cronExpression">Cron expression that represents the recurring schedule.</param>
        /// <param name="queue">Optional queue to run the task.</param>
        /// <param name="timeZone">Optional time zone to use for the cron expression. Should default to Utc.</param>
        void AddOrUpdate<TService>(
            string id, 
            Expression<Func<TService, Task>> expression, 
            string cronExpression, 
            string queue = null,
            TimeZoneInfo timeZone = null);

        /// <summary>
        /// Execute an established recurring task immediately given its unique id.
        /// </summary>
        /// <param name="id"></param>
        void Trigger(string id);

        /// <summary>
        /// Remove an established recurring task from the schedule given its unique id.
        /// </summary>
        /// <param name="id"></param>
        void Remove(string id);
    }
}
