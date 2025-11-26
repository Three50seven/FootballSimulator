using Common.Core.Domain;
using System.Collections.Generic;

namespace Common.Core.Services
{
    /// <summary>
    /// Empty implementation to supply no-cache ICache.
    /// </summary>
    public sealed class EmptyCache : ICache
    {
        public void Clear()
        {

        }

        public bool Exists(string key)
        {
            return false;
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public IEnumerable<SelectItem> GetAll()
        {
            return new List<SelectItem>();
        }

        public void Remove(string key)
        {

        }

        public void RemoveAll(string keyContains)
        {

        }

        public void Set(object value, string key)
        {

        }

        public void Set(object value, string key, string profile)
        {

        }

        public void Set(object value, string key, double minutes)
        {

        }

        public void SetNoExpire(object value, string key)
        {

        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            return false;
        }
    }
}
