namespace Common.Core.Domain
{
    public class ProcessParameter : KeyValueEntity
    {
        protected ProcessParameter() { }

        public ProcessParameter(Process process, KeyValuePair<string, string> keyValuePair)
            : base(keyValuePair)
        {
            ProcessId = process.Id;
            Process = process;
        }

        public int ProcessId { get; private set; }
        public Process? Process { get; private set; }
    }
}
