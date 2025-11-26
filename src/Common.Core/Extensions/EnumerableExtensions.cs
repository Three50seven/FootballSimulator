using Common.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Whether any items in the list are null.
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool AnyNull(this IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item == null)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks for null and length.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool HasItems(this Array array)
        {
            return array != null && array.Length > 0;
        }

        /// <summary>
        /// Checks for null and <see cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool HasItems<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }

        /// <summary>
        /// Filter list to distinct values by a specific key <paramref name="keySelector"/>.
        /// Reference - http://stackoverflow.com/a/489421 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Reverse and then filter list to distinct values by a specific key <paramref name="keySelector"/>.
        /// Reference - http://stackoverflow.com/a/489421 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctByReverse<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return DistinctBy(source.Reverse(), keySelector);
        }

        /// <summary>
        /// Extends <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
        /// and <see cref="Enumerable.OrderByDescending{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
        /// by allowing dynamic direction option <paramref name="direction"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortDirectionOption direction)
        {
            switch (direction)
            {
                case SortDirectionOption.Ascending:
                    return source.OrderBy(keySelector);
                case SortDirectionOption.Descending:
                    return source.OrderByDescending(keySelector);
                default:
                    throw new InvalidOperationException("OrderBy SortDirection not valid.");
            }
        }

        /// <summary>
        /// Extends <see cref="Enumerable.ThenBy{TSource, TKey}(IOrderedEnumerable{TSource}, Func{TSource, TKey})"/>
        /// and <see cref="Enumerable.ThenByDescending{TSource, TKey}(IOrderedEnumerable{TSource}, Func{TSource, TKey})"/>
        /// by allowing dynamic direction option <paramref name="direction"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortDirectionOption direction)
        {
            switch (direction)
            {
                case SortDirectionOption.Ascending:
                    return source.ThenBy(keySelector);
                case SortDirectionOption.Descending:
                    return source.ThenByDescending(keySelector);
                default:
                    throw new InvalidOperationException("ThenBy SortDirection not valid.");
            }
        }

        private static Random _random = new Random();

        /// <summary>
        /// Pick a random item from the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            Guard.IsNotNull(source, nameof(source));

            int randomIndex = _random.Next(source.Count());
            return source.ElementAt(randomIndex);
        }

        /// <summary>
        /// Pick random list of items from source list <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Source list.</param>
        /// <param name="count">Number of random items to return.</param>
        /// <returns></returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return Shuffle(source).Take(count);
        }

        /// <summary>
        /// Shuffle items in list into a random order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source?.OrderBy(x => Guid.NewGuid());
        }

        /// <summary>
        /// Apply newly selected relational ids <paramref name="selectedRelationalIds"/> against existing list
        /// of "bridge table" entities <paramref name="bridgeEntities"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bridgeEntities">Existing list of "bridge table" entities. Can be empty.</param>
        /// <param name="selectedRelationalIds">New list of relational ids to apply to the list.</param>
        /// <param name="relationalIdSelector">Expression to get the relational Id field.</param>
        /// <param name="constructFunc">Construction callback function to create entity <typeparamref name="T"/>.</param>
        /// <returns></returns>
        public static IEnumerable<T> AdjustRelationalBridgeEntities<T>(
            this IList<T> bridgeEntities,
            int[] selectedRelationalIds,
            Func<T, int> relationalIdSelector,
            Func<int, T> constructFunc)
            where T : class
        {
            Guard.IsNotNull(bridgeEntities, nameof(bridgeEntities));

            // has existing relations
            if (bridgeEntities.Any())
            {
                // new relations ids selected
                if (selectedRelationalIds != null && selectedRelationalIds.Length > 0)
                {
                    // build list to remove
                    var removedBridgeEntities = new List<T>();
                    foreach (var bridgeEntity in bridgeEntities)
                    {
                        if (!selectedRelationalIds.Contains(relationalIdSelector.Invoke(bridgeEntity)))
                            removedBridgeEntities.Add(bridgeEntity);
                    }

                    // remove
                    foreach (var removedBridgeEntity in removedBridgeEntities)
                    {
                        bridgeEntities.Remove(removedBridgeEntity);
                    }

                    // add any new
                    for (int i = 0; i < selectedRelationalIds.Length; i++)
                    {
                        if (!bridgeEntities.Select(relationalIdSelector).Any(relationalId => relationalId == selectedRelationalIds[i]))
                            bridgeEntities.Add(constructFunc.Invoke(selectedRelationalIds[i]));
                    }
                }
                else
                {
                    // remove all relations since none selected
                    bridgeEntities.Clear();
                }
            }
            else if (selectedRelationalIds != null && selectedRelationalIds.Length > 0)
            {
                // add all selected as new
                foreach (var entityId in selectedRelationalIds)
                {
                    bridgeEntities.Add(constructFunc.Invoke(entityId));
                }
            }

            return bridgeEntities;
        }

        /// <summary>
        /// Add to the collection only if not already existing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">Existing list of items.</param>
        /// <param name="item">Item to add.</param>
        /// <param name="predicate">Callback function on filtering the list to determine if the item is unique.</param>
        public static void AddUnique<T>(this ICollection<T> list, T item, Func<T, bool> predicate)
        {
            if (list == null || item == null)
                return;

            if (!list.Any(predicate))
                list.Add(item);
        }

        /// <summary>
        /// Add any unique items to the existing list. Calls into <see cref="AddUnique{T}(ICollection{T}, T, Func{T, bool})"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">Existing list of items.</param>
        /// <param name="items">New list of items to add.</param>
        /// <param name="predicate">Callback function on filtering the list to determine if the item is unique.</param>
        public static void AddUniqueRange<T>(this ICollection<T> list, IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (list == null || items == null || !items.Any())
                return;

            foreach (var item in items)
            {
                AddUnique(list, item, predicate);
            }
        }

        /// <summary>
        /// Returns an enumerable that has been filled to a supplied required count 
        /// if provided enumerable list has a count less than the required count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="requiredCount"></param>
        /// <returns></returns>
        public static IEnumerable<T> FillToRequiredLength<T>(this IEnumerable<T> list, int requiredCount)
           where T : struct
        {
            if (list.Count() > requiredCount)
                return list;

            var filledList = new List<T>(list);
            do
            {
                filledList.Add(default);
            }
            while (filledList.Count < requiredCount);

            return filledList;
        }

        /// <summary>
        /// Page through an enumerable list with callback action for each item in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Enumerable list to page through.</param>
        /// <param name="callbackPerBatch">Callback per item in the list.</param>
        /// <param name="batchSize">Batch/page size to take/skip as part of the paging process.</param>
        /// <param name="cancellationToken">Optional cancellation token. Token is checked at the top of each page start.</param>
        public static void ForEachBatched<T>(
            this IEnumerable<T> items,
            Action<IEnumerable<T>> callbackPerBatch,
            int batchSize = 100,
            CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(items, nameof(items));
            Guard.IsNotNull(callbackPerBatch, nameof(callbackPerBatch));
            Guard.IsPositive(batchSize, nameof(batchSize));

            while (items.Any())
            {
                cancellationToken.ThrowIfCancellationRequested();

                callbackPerBatch(items.Take(batchSize));

                items = items.Skip(batchSize);
            }
        }

        /// <summary>
        /// Page through an enumerable list with async callback action for each item in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Enumerable list to page through.</param>
        /// <param name="callbackPerBatch">Callback per item in the list.</param>
        /// <param name="batchSize">Batch/page size to take/skip as part of the paging process.</param>
        /// <param name="cancellationToken">Optional cancellation token. Token is checked at the top of each page start.</param>
        /// <returns></returns>
        public static async Task ForEachBatchedAsync<T>(
            this IEnumerable<T> items,
            Func<IEnumerable<T>, Task> callbackPerBatch,
            int batchSize = 100,
            CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(items, nameof(items));
            Guard.IsNotNull(callbackPerBatch, nameof(callbackPerBatch));
            Guard.IsPositive(batchSize, nameof(batchSize));

            while (items.Any())
            {
                cancellationToken.ThrowIfCancellationRequested();

                await callbackPerBatch(items.Take(batchSize));

                items = items.Skip(batchSize);
            }
        }

        /// <summary>
        /// Traverse list and flatten children based on <paramref name="childSelector"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="childSelector">Property on list that indicates self-referencing children.</param>
        /// <param name="useStack">Whether process should utilize <see cref="Stack{T}"/> rather than recursion.</param>
        /// <returns></returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector, bool useStack = false)
        {
            if (useStack)
            {
                var stack = new Stack<T>(items);
                while (stack.Any())
                {
                    var next = stack.Pop();
                    yield return next;
                    foreach (var child in childSelector(next))
                    {
                        stack.Push(child);
                    }
                }
            }
            else
            {
                foreach (T item in items)
                {
                    yield return item;
                    IEnumerable<T> children = childSelector(item);
                    foreach (T child in Traverse(children, childSelector))
                    {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// Apply a predicate Where to an enumerable if a given condition is met.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(
            this IEnumerable<T> query,
            bool condition,
            Func<T, bool> predicate)
        {
            if (condition)
                return query.Where(predicate);
            else
                return query;
        }
    }
}
