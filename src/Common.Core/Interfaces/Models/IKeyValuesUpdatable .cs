using System.Collections.Generic;

namespace Common.Core
{
    public interface IKeyValuesUpdatable
    {
        void UpdateValues(IEnumerable<KeyValuePair<string, string>> keyValues);
    }
}
