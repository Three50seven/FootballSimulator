using System;

namespace Common.Core.Domain
{
    public class ProcessRetry : Entity<int>
    {
        protected ProcessRetry() { }

        public ProcessRetry(Process process, int attempt, string reason = null!)
        {
            Process = process;
            Attempt = attempt;
            Reason = reason.SetEmptyToNull();
            CreatedDate = DateTime.UtcNow;
        }

        public int ProcessId { get; private set; }
        public Process? Process { get; private set; }

        public int Attempt { get; private set; }
        public string? Reason { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? StartedDate { get; internal set; }
        public DateTime? FinishedDate { get; internal set; }
    }
}
