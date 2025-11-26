namespace Common.Core.Domain
{
    public class ProcessResult : KeyValueEntity
    {
        protected ProcessResult() { }

        public ProcessResult(Process process, string key, string value)
            : base(key, value)
        {
            ProcessId = process?.Id ?? 0;
            Process = process ?? throw new ArgumentNullException(nameof(process));
        }

        public ProcessResult(int processId, string key, string value)
            : base(key, value)
        {
            ProcessId = processId;
        }

        public int ProcessId { get; private set; }
        public Process? Process { get; private set; }
    }
}
