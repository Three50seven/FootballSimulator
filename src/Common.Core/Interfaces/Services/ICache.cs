using Common.Core.Domain;
using System.Collections.Generic;

namespace Common.Core
{
    public interface ICache
    {
        bool Exists(string key);
        void Set(object value, string key);
        void Set(object value, string key, string profile);
        void Set(object value, string key, double minutes);
        void SetNoExpire(object value, string key);
        void Remove(string key);
        void RemoveAll(string keyContains);
        void Clear();
        T Get<T>(string key);
        bool TryGet<T>(string key, out T value);
        IEnumerable<SelectItem> GetAll();
    }
}
