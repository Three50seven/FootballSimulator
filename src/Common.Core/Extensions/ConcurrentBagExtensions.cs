using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core
{
    public static class ConcurrentBagExtensions
    {
        /// <summary>
        /// Add one or many items to the bag. Simply calls <see cref="ConcurrentBag{T}.Add(T)"/> in a loop.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bag"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ConcurrentBag<T> bag, IEnumerable<T> items)
        {
            if (items == null || !items.Any())
                return;

            foreach (var item in items)
            {
                bag.Add(item);
            }
        }
    }
}
