using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Core.Interfaces
{
    /// <summary>
    /// Abstraction for queuing basic tasks/actions or simple services within a transaction.
    /// </summary>
    public interface ITaskQueue : ITransactionalQueue
    {
        /// <summary>
        /// Queue anonymous task, optionally at a scheduled time and/or under a specific queue.
        /// </summary>
        /// <param name="expression">Task/action to queue.</param>
        /// <param name="scheduledDate">Optional date/time for the task to execute.</param>
        /// <param name="queue">Optional queue on which to place the task.</param>
        void Enqueue(Expression<Func<Task>> expression, DateTime? scheduledDate = null, string queue = null);

        /// <summary>
        /// Queue service task, optionally at a scheduled time and/or under a specific queue.
        /// </summary>
        /// <typeparam name="TService">Service to be used to run the task.</typeparam>
        /// <param name="expression">Task on service to execute.</param>
        /// <param name="scheduledDate">Optional date/time for the task to execute.</param>
        /// <param name="queue">Optional queue on which to place the task.</param>
        void Enqueue<TService>(Expression<Func<TService, Task>> expression, DateTime? scheduledDate = null, string queue = null);
    }
}
