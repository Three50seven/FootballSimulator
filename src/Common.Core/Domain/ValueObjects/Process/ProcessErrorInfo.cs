using System;

namespace Common.Core.DTOs
{
    public class ProcessErrorInfo
    {
        public ProcessErrorInfo(Guid errorId, Exception ex)
        {
            Guid = errorId;
            Exception = ex;
        }

        public Guid Guid { get; private set; }
        public Exception Exception { get; private set; }
    }
}
