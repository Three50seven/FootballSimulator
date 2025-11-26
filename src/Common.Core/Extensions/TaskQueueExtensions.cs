using Common.Core.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class TaskQueueExtensions
    {
        /// <summary>
        /// Schedule an anonymous task to a queue.
        /// </summary>
        /// <param name="taskQueue"></param>
        /// <param name="expression">Task/action to queue.</param>
        /// <param name="datetime">Required future date/time for the task to execute.</param>
        /// <param name="queue">Optional queue on which to place the task.</param>
        public static void Schedule(this ITaskQueue taskQueue, Expression<Func<Task>> expression, DateTime datetime, string queue = null)
        {
            taskQueue?.Enqueue(expression, datetime, queue);
        }

        /// <summary>
        /// Schedule service task to a queue.
        /// </summary>
        /// <typeparam name="TService">Service to be used to run the task.</typeparam>
        /// <param name="taskQueue"></param>
        /// <param name="expression">Task on service to execute.</param>
        /// <param name="datetime">Required future date/time for the task to execute.</param>
        /// <param name="queue">Optional queue on which to place the task.</param>
        public static void Schedule<TService>(this ITaskQueue taskQueue, Expression<Func<TService, Task>> expression, DateTime datetime, string queue = null)
        {
            taskQueue?.Enqueue(expression, datetime, queue);
        }
    }
}
