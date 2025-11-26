using System.Collections.Generic;

namespace Common.AspNetCore
{
    internal class ModelStateTransferValue
    {
        public string Key { get; set; }
        public string AttemptedValue { get; set; }
        public object RawValue { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; } = new List<string>();
    }
}
