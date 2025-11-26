using Common.Core.Domain;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Core.Interfaces
{
    /// <summary>
    /// Simple service abstraction for executing an action based on a set of custom arguments.
    /// Designed to be tied to a process implementation of <see cref="Process"/>.
    /// Required for a service to be queued using <see cref="IProcessQueue"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProcessService<T> 
        where T : ProcessArguments
    {
        /// <summary>
        /// Perform the service action given a set of specified arguments.
        /// ProcessContext is supplied to include the Guid of the Process record for the specific execution of the service.
        /// </summary>
        /// <param name="arguments">Custom arguments needed to perform the action.</param>
        /// <param name="context">Process context holding the process indentifiable information for this action.</param>
        /// <param name="token">Optional cancellation token for stopping running task.</param>
        /// <returns></returns>
        [DisplayName("{0}")] // display name used to for displaying friendly name of the task (in Hangfire, this will display the args ToString in the dashboard list)
        Task ExecuteAsync(T arguments, ProcessContext context, CancellationToken token = default);
    }
}
