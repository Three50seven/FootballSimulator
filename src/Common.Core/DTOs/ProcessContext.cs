using Common.Core.Domain;
using System;

namespace Common.Core
{
    /// <summary>
    /// Simple object to represent identifiable information for the process <see cref="Process"/>.
    /// Supplied into <see cref="Common.Core.Interfaces.IProcessService{T}.ExecuteAsync(T, ProcessContext, System.Threading.CancellationToken)"/> 
    /// as a means for the process service to connect with its process record.
    /// </summary>
    public class ProcessContext
    {
        protected ProcessContext() { }

        public ProcessContext(Guid guid)
        {
            Guid = guid;
        }

        /// <summary>
        /// Identifier used to find the process record inheriting <see cref="Process"/>.
        /// </summary>
        public Guid Guid { get; private set; }
    }
}
