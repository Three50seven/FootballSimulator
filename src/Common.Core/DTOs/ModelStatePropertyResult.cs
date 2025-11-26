using System.Collections.Generic;

namespace Common.Core.DTOs
{
    public class ModelStatePropertyResult
    {
        public string Key { get; set; }
        public string AttemptedValue { get; set; }
        public object RawValue { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; } = new List<string>();
    }
}
