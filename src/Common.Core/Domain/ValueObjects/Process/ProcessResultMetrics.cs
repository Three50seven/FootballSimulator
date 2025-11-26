using System.Collections.Generic;
using System.Linq;

namespace Common.Core.Domain
{
    public abstract class ProcessResultMetrics : ValueObject<ProcessResultMetrics>
    {
        public ProcessResultMetrics() { }

        public ProcessResultMetrics(IEnumerable<ProcessResult> resultValues)
            : this(resultValues?.Select(pr => pr.ToKeyValuePair()))
        {

        }

        public ProcessResultMetrics(IEnumerable<KeyValuePair<string, object>> values)
        {
            if (values != null)
                SetValues(values);
        }

        protected abstract void SetValues(IEnumerable<KeyValuePair<string, object>> values);
        public abstract IEnumerable<KeyValuePair<string, object>> ToKeyValuesList();

        public virtual IEnumerable<KeyValuePair<string, string>> ToKeyValueStrings()
        {
            return ToKeyValuesList()?.Select(arg => new KeyValuePair<string, string>(arg.Key, arg.Value?.ToString()));
        }
    }
}
