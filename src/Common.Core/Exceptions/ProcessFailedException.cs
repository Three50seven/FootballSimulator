using System;

namespace Common.Core
{
    [Serializable]
    public class ProcessFailedException : Exception
    {
        public ProcessFailedException(int processId, Exception inner)
            : base($"Process ({processId}) failed. See inner exception for details.", inner)
        {

        }

        public int ProcessId { get; private set; }
    }
}
