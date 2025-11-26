using System.Collections.Generic;

namespace Common.Core.Domain
{
    public static class KeyValueEntityExtensions
    {
        public static KeyValuePair<string, object> ToKeyValuePair(this KeyValueEntity entity)
        {
            return new KeyValuePair<string, object>(entity.Key, entity.Value);
        }
    }
}
