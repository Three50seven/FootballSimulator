using Common.Core.Validation;
using System.Collections;

namespace Common.Core
{
    public static class EnumerableExtensions
    {
        private static Random _random = new Random();

        //
        // Summary:
        //     Whether any items in the list are null.
        //
        // Parameters:
        //   enumerable:
        public static bool AnyNull(this IEnumerable enumerable)
        {
            foreach (object item in enumerable)
            {
                if (item == null)
                {
                    return true;
                }
            }

            return false;
        }

        //
        // Summary:
        //     Checks for null and length.
        //
        // Parameters:
        //   array:
        public static bool HasItems(this Array array)
        {
            return array != null && array.Length > 0;
        }

        //
        // Summary:
        //     Checks for null and System.Linq.Enumerable.Any``1(System.Collections.Generic.IEnumerable{``0}).
        //
        //
        // Parameters:
        //   list:
        //
        // Type parameters:
        //   T:
        public static bool HasItems<T>(this IEnumerable<T> list)
        {
            return list?.Any() ?? false;
        }

        //
        // Summary:
        //     Filter list to distinct values by a specific key keySelector. Reference - http://stackoverflow.com/a/489421
        //
        //
        // Parameters:
        //   source:
        //
        //   keySelector:
        //
        // Type parameters:
        //   TSource:
        //
        //   TKey:
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

        //
        // Summary:
        //     Reverse and then filter list to distinct values by a specific key keySelector.
        //     Reference - http://stackoverflow.com/a/489421
        //
        // Parameters:
        //   source:
        //
        //   keySelector:
        //
        // Type parameters:
        //   TSource:
        //
        //   TKey:
        public static IEnumerable<TSource> DistinctByReverse<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.Reverse().DistinctBy(keySelector);
        }

        //
        // Summary:
        //     Extends System.Linq.Enumerable.OrderBy``2(System.Collections.Generic.IEnumerable{``0},System.Func{``0,``1})
        //     and System.Linq.Enumerable.OrderByDescending``2(System.Collections.Generic.IEnumerable{``0},System.Func{``0,``1})
        //     by allowing dynamic direction option direction.
        //
        // Parameters:
        //   source:
        //
        //   keySelector:
        //
        //   direction:
        //
        // Type parameters:
        //   TSource:
        //
        //   TKey:
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortDirectionOption direction)
        {
            return direction switch
            {
                SortDirectionOption.Ascending => source.OrderBy(keySelector),
                SortDirectionOption.Descending => source.OrderByDescending(keySelector),
                _ => throw new InvalidOperationException("OrderBy SortDirection not valid."),
            };
        }

        //
        // Summary:
        //     Extends System.Linq.Enumerable.ThenBy``2(System.Linq.IOrderedEnumerable{``0},System.Func{``0,``1})
        //     and System.Linq.Enumerable.ThenByDescending``2(System.Linq.IOrderedEnumerable{``0},System.Func{``0,``1})
        //     by allowing dynamic direction option direction.
        //
        // Parameters:
        //   source:
        //
        //   keySelector:
        //
        //   direction:
        //
        // Type parameters:
        //   TSource:
        //
        //   TKey:
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortDirectionOption direction)
        {
            return direction switch
            {
                SortDirectionOption.Ascending => source.ThenBy(keySelector),
                SortDirectionOption.Descending => source.ThenByDescending(keySelector),
                _ => throw new InvalidOperationException("ThenBy SortDirection not valid."),
            };
        }

        //
        // Summary:
        //     Pick a random item from the list.
        //
        // Parameters:
        //   source:
        //
        // Type parameters:
        //   T:
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            Guard.IsNotNull(source, "source");
            int index = _random.Next(source.Count());
            return source.ElementAt(index);
        }

        //
        // Summary:
        //     Pick random list of items from source list source.
        //
        // Parameters:
        //   source:
        //     Source list.
        //
        //   count:
        //     Number of random items to return.
        //
        // Type parameters:
        //   T:
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            var shuffled = source.Shuffle() ?? Enumerable.Empty<T>();
            return shuffled.Take(count);
        }

        //
        // Summary:
        //     Shuffle items in list into a random order.
        //
        // Parameters:
        //   source:
        //
        // Type parameters:
        //   T:
        public static IEnumerable<T>? Shuffle<T>(this IEnumerable<T> source)
        {
            return source?.OrderBy((T x) => Guid.NewGuid());
        }

        //
        // Summary:
        //     Apply newly selected relational ids selectedRelationalIds against existing list
        //     of "bridge table" entities bridgeEntities.
        //
        // Parameters:
        //   bridgeEntities:
        //     Existing list of "bridge table" entities. Can be empty.
        //
        //   selectedRelationalIds:
        //     New list of relational ids to apply to the list.
        //
        //   relationalIdSelector:
        //     Expression to get the relational Id field.
        //
        //   constructFunc:
        //     Construction callback function to create entity T.
        //
        // Type parameters:
        //   T:
        public static IEnumerable<T> AdjustRelationalBridgeEntities<T>(this IList<T> bridgeEntities, int[] selectedRelationalIds, Func<T, int> relationalIdSelector, Func<int, T> constructFunc) where T : class
        {
            Guard.IsNotNull(bridgeEntities, "bridgeEntities");
            if (bridgeEntities.Any())
            {
                if (selectedRelationalIds != null && selectedRelationalIds.Length != 0)
                {
                    List<T> list = [];
                    foreach (T bridgeEntity in bridgeEntities)
                    {
                        if (!selectedRelationalIds.Contains(relationalIdSelector(bridgeEntity)))
                        {
                            list.Add(bridgeEntity);
                        }
                    }

                    foreach (T item in list)
                    {
                        bridgeEntities.Remove(item);
                    }

                    int i;
                    for (i = 0; i < selectedRelationalIds.Length; i++)
                    {
                        if (!bridgeEntities.Select(relationalIdSelector).Any((int relationalId) => relationalId == selectedRelationalIds[i]))
                        {
                            bridgeEntities.Add(constructFunc(selectedRelationalIds[i]));
                        }
                    }
                }
                else
                {
                    bridgeEntities.Clear();
                }
            }
            else if (selectedRelationalIds != null && selectedRelationalIds.Length != 0)
            {
                int[] array = selectedRelationalIds;
                foreach (int arg in array)
                {
                    bridgeEntities.Add(constructFunc(arg));
                }
            }

            return bridgeEntities;
        }

        //
        // Summary:
        //     Add to the collection only if not already existing.
        //
        // Parameters:
        //   list:
        //     Existing list of items.
        //
        //   item:
        //     Item to add.
        //
        //   predicate:
        //     Callback function on filtering the list to determine if the item is unique.
        //
        // Type parameters:
        //   T:
        public static void AddUnique<T>(this ICollection<T> list, T item, Func<T, bool> predicate)
        {
            if (list != null && item != null && !list.Any(predicate))
            {
                list.Add(item);
            }
        }

        //
        // Summary:
        //     Add any unique items to the existing list. Calls into Common.Core.EnumerableExtensions.AddUnique``1(System.Collections.Generic.ICollection{``0},``0,System.Func{``0,System.Boolean}).
        //
        //
        // Parameters:
        //   list:
        //     Existing list of items.
        //
        //   items:
        //     New list of items to add.
        //
        //   predicate:
        //     Callback function on filtering the list to determine if the item is unique.
        //
        // Type parameters:
        //   T:
        public static void AddUniqueRange<T>(this ICollection<T> list, IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (list == null || items == null || !items.Any())
            {
                return;
            }

            foreach (T item in items)
            {
                list.AddUnique(item, predicate);
            }
        }

        //
        // Summary:
        //     Returns an enumerable that has been filled to a supplied required count if provided
        //     enumerable list has a count less than the required count.
        //
        // Parameters:
        //   list:
        //
        //   requiredCount:
        //     Type parameters:
        //   T:
        public static IEnumerable<T> FillToRequiredLength<T>(this IEnumerable<T> list, int requiredCount) where T : struct
        {
            if (list.Count() > requiredCount)
            {
                return list;
            }

            List<T> list2 = [.. list];
            do
            {
                list2.Add(default);
            }
            while (list2.Count < requiredCount);
            return list2;
        }

        //
        // Summary:
        //     Page through an enumerable list with callback action for each item in the list.
        //
        //
        // Parameters:
        //   items:
        //     Enumerable list to page through.
        //
        //   callbackPerBatch:
        //     Callback per item in the list.
        //
        //   batchSize:
        //     Batch/page size to take/skip as part of the paging process.
        //
        //   cancellationToken:
        //     Optional cancellation token. Token is checked at the top of each page start.
        //
        //
        // Type parameters:
        //   T:
        public static void ForEachBatched<T>(this IEnumerable<T> items, Action<IEnumerable<T>> callbackPerBatch, int batchSize = 100, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guard.IsNotNull(items, "items");
            Guard.IsNotNull(callbackPerBatch, "callbackPerBatch");
            Guard.IsPositive(batchSize, "batchSize");
            while (items.Any())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callbackPerBatch(items.Take(batchSize));
                items = items.Skip(batchSize);
            }
        }

        //
        // Summary:
        //     Page through an enumerable list with async callback action for each item in the
        //     list.
        //
        // Parameters:
        //   items:
        //     Enumerable list to page through.
        //
        //   callbackPerBatch:
        //     Callback per item in the list.
        //
        //   batchSize:
        //     Batch/page size to take/skip as part of the paging process.
        //
        //   cancellationToken:
        //     Optional cancellation token. Token is checked at the top of each page start.
        //
        //
        // Type parameters:
        //   T:
        public static async Task ForEachBatchedAsync<T>(this IEnumerable<T> items, Func<IEnumerable<T>, Task> callbackPerBatch, int batchSize = 100, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guard.IsNotNull(items, "items");
            Guard.IsNotNull(callbackPerBatch, "callbackPerBatch");
            Guard.IsPositive(batchSize, "batchSize");
            while (items.Any())
            {
                cancellationToken.ThrowIfCancellationRequested();
                await callbackPerBatch(items.Take(batchSize));
                items = items.Skip(batchSize);
            }
        }

        //
        // Summary:
        //     Traverse list and flatten children based on childSelector.
        //
        // Parameters:
        //   items:
        //
        //   childSelector:
        //     Property on list that indicates self-referencing children.
        //
        //   useStack:
        //     Whether process should utilize System.Collections.Generic.Stack`1 rather than
        //     recursion.
        //
        // Type parameters:
        //   T:
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector, bool useStack = false)
        {
            if (useStack)
            {
                Stack<T> stack = new Stack<T>(items);
                while (stack.Any())
                {
                    T next = stack.Pop();
                    yield return next;
                    foreach (T child in childSelector(next))
                    {
                        stack.Push(child);
                    }
                }

                yield break;
            }

            foreach (T item in items)
            {
                yield return item;
                IEnumerable<T> children = childSelector(item);
                foreach (T item2 in children.Traverse(childSelector))
                {
                    yield return item2;
                }
            }
        }

        //
        // Summary:
        //     Apply a predicate Where to an enumerable if a given condition is met.
        //
        // Parameters:
        //   query:
        //
        //   condition:
        //
        //   predicate:
        //
        // Type parameters:
        //   T:
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> predicate)
        {
            if (condition)
            {
                return query.Where(predicate);
            }

            return query;
        }
    }
}
