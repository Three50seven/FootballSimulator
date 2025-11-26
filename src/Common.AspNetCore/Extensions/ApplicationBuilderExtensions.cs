using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Common.Core.Domain;
using Common.Core.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Schedule a recurring Process service.
        /// Requires registration of <see cref="IProcessRecurringScheduler"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="app"></param>
        /// <param name="id">Unique identifier for this process service recurring task.</param>
        /// <param name="arguments">Custom arguments needed to perform the process service action.</param>
        /// <param name="cronExpression">Cron expression that represents the recurring schedule.</param>
        /// <param name="timeZone">Optional time zone to use for the cron expression. Defaults to Utc.</param>
        /// <returns></returns>
        public static IApplicationBuilder ScheduleProcess<TService, TArgs>(
            this IApplicationBuilder app,
            string id,
            TArgs arguments,
            string cronExpression,
            TimeZoneInfo timeZone = null)
            where TService : IProcessService<TArgs>
            where TArgs : ProcessArguments
        {
            app.ApplicationServices.GetRequiredService<IProcessRecurringScheduler>()
                .AddOrUpdate<TService, TArgs>(id, arguments, cronExpression, timeZone);

            return app;
        }

        /// <summary>
        /// Schedule a generic recurring task/action.
        /// Requires registration of <see cref="ITaskRecurringScheduler"/>.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="id">Unique identifier for this process service recurring task.</param>
        /// <param name="expression">Task to execute.</param>
        /// <param name="cronExpression">Cron expression that represents the recurring schedule.</param>
        /// <param name="queue">Optional queue to run the task.</param>
        /// <param name="timeZone">Optional time zone to use for the cron expression. Defaults to Utc.</param>
        /// <returns></returns>
        public static IApplicationBuilder ScheduleTask(
            this IApplicationBuilder app,
            string id,
            Expression<Action> expression,
            string cronExpression,
            string queue = null,
            TimeZoneInfo timeZone = null)
        {
            app.ApplicationServices.GetRequiredService<ITaskRecurringScheduler>()
                .AddOrUpdate(id, expression, cronExpression, queue, timeZone);

            return app;
        }

        /// <summary>
        /// Schedule a generic recurring task service class/interface.
        /// Requires registration of <see cref="ITaskRecurringScheduler"/>.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="id">Unique identifier for this process service recurring task.</param>
        /// <param name="expression">Task service expression to execute.</param>
        /// <param name="cronExpression">Cron expression that represents the recurring schedule.</param>
        /// <param name="queue">Optional queue to run the task.</param>
        /// <param name="timeZone">Optional time zone to use for the cron expression. Defaults to Utc.</param>
        /// <returns></returns>
        public static IApplicationBuilder ScheduleTask(
            this IApplicationBuilder app,
            string id,
            Expression<Func<Task>> expression,
            string cronExpression,
            string queue = null,
            TimeZoneInfo timeZone = null)
        {
            app.ApplicationServices.GetRequiredService<ITaskRecurringScheduler>()
                .AddOrUpdate(id, expression, cronExpression, queue, timeZone);

            return app;
        }


        /// <summary>
        /// Schedule a generic recurring task service class/interface of type <typeparamref name="TService"/>.
        /// Requires registration of <see cref="ITaskRecurringScheduler"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="app"></param>
        /// <param name="id">Unique identifier for this process service recurring task.</param>
        /// <param name="expression">Task service expression to execute.</param>
        /// <param name="cronExpression">Cron expression that represents the recurring schedule.</param>
        /// <param name="queue">Optional queue to run the task.</param>
        /// <param name="timeZone">Optional time zone to use for the cron expression. Defaults to Utc.</param>
        /// <returns></returns>
        public static IApplicationBuilder ScheduleTask<TService>(
            this IApplicationBuilder app,
            string id,
            Expression<Func<TService, Task>> expression,
            string cronExpression,
            string queue = null,
            TimeZoneInfo timeZone = null)
        {
            app.ApplicationServices.GetRequiredService<ITaskRecurringScheduler>()
                                   .AddOrUpdate<TService>(id, expression, cronExpression, queue, timeZone);

            return app;
        }
    }
}
