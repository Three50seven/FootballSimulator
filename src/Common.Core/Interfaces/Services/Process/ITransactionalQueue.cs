using System;
using System.Collections.Generic;

namespace Common.Core.Interfaces
{
    /// <summary>
    /// Abstraction for a transaction-based queue.
    /// </summary>
    public interface ITransactionalQueue
    {
        /// <summary>
        /// Commit items found in a queue and return results as a string result for each queued item.
        /// </summary>
        /// <param name="delay">Optional delay to enforce on the committing of jobs.</param>
        /// <returns>String result for each queued item that was committed. In Hangfire, this is the JobId for each job committed.</returns>
        IEnumerable<string> Commit(TimeSpan? delay = null);
    }
}
