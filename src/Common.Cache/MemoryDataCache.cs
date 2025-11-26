using Common.Core;
using Common.Core.Domain;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Common.Cache
{
    /// <summary>
    /// Wrapper around <see cref="IMemoryCache"/> for caching data objects.
    /// </summary>
    public class MemoryDataCache : ICache
    {
        private readonly ConcurrentDictionary<string, object> _items = new();

        public MemoryDataCache(IMemoryCache memoryCache, DataCacheSettings settings)
        {
            MemoryCache = memoryCache;
            Settings = settings;
        }

        protected IMemoryCache MemoryCache { get; }
        protected DataCacheSettings Settings { get; }

        protected virtual double DefaultCacheRetention => Settings.DefaultRetention > 0 ? Settings.DefaultRetention
                                                                : CacheConstants.DefaultRetentionTime;

        public virtual void Clear()
        {
            var keys = _items.Select(c => c.Key);

            foreach (var key in keys)
            {
                MemoryCache.Remove(key);
            }

            _items.Clear();
        }

        public virtual bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;
            return MemoryCache.TryGetValue(key, out _);
        }

        public virtual T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return MemoryCache.Get<T>(key)!;
        }

        public virtual IEnumerable<SelectItem> GetAll()
        {
            var items = new List<SelectItem>();

            var keys = _items.Select(x => x.Key);

            foreach (var key in keys)
            {
                if (TryGet(key, out object value))
                {
                    items.Add(new SelectItem(key, value.ToString()!));
                }
            }

            return items;
        }

        public virtual void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            _items.TryRemove(key, out _);
            MemoryCache.Remove(key);
        }

        public virtual void RemoveAll(string keyContains)
        {
            if (string.IsNullOrWhiteSpace(keyContains))
                return;

            keyContains = keyContains.ToLower();

            var keys = _items.Where(x => x.Key.Contains(keyContains, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Key);

            foreach (var key in keys)
            {
                _items.TryRemove(key, out object? value);
                MemoryCache.Remove(key);
            }
        }

        public virtual void Set(object value, string key)
        {
            Set(value, key, DefaultCacheRetention);
        }

        public virtual void Set(object value, string key, string profile)
        {
            if (string.IsNullOrWhiteSpace(profile))
                return;

            if (!Settings.Profiles.TryGetValue(profile, out TimeSpan profileTimeSpan))
                throw new NullReferenceException($"DataCache profile '{profile}' was not found.");

            Set(value, key, profileTimeSpan.TotalMinutes);
        }

        public virtual void Set(object value, string key, double minutes)
        {
            if (value == null || string.IsNullOrWhiteSpace(key))
                return;

            _items.TryAdd(key, value);
            MemoryCache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes)));
        }

        public virtual void SetNoExpire(object value, string key)
        {
            if (value == null || string.IsNullOrWhiteSpace(key))
                return;

            _items.TryAdd(key, value.ToString()!);
            MemoryCache.Set(key, value);
        }

        public virtual bool TryGet<T>(string key, out T value)
        {
            return MemoryCache.TryGetValue(key, out value!);
        }
    }
}
