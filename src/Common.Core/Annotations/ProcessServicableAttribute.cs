namespace Common.Core
{
    /// <summary>
    /// Required attribute for <see cref="Common.Core.Interfaces.IProcessService{T}"/> interfaces 
    /// when queuing process services using <see cref="Common.Core.Interfaces.IProcessQueue"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class ProcessServicableAttribute : Attribute
    {
        /// <summary>
        /// Required attribute for <see cref="Common.Core.Interfaces.IProcessService{T}"/> interfaces 
        /// when queuing process services using <see cref="Common.Core.Interfaces.IProcessQueue"/>.
        /// </summary>
        /// <param name="processTypeId">Type of the process. Required. Unique to the running application.</param>
        /// <param name="queue">Optional queue for which the process service should execute. Defaults to a default queue.</param>
        public ProcessServicableAttribute(int processTypeId, string queue = null!)
        {
            ProcessTypeId = processTypeId;
            Queue = queue;
        }

        /// <summary>
        /// Type of the process. Required. Unique to the running application.
        /// </summary>
        public int ProcessTypeId { get; }

        /// <summary>
        /// Optional queue for which the process service should execute. Defaults to a default queue.
        /// </summary>
        public string Queue { get; }
    }
}
