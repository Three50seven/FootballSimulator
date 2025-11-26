using System.Collections.Generic;

namespace Common.Core
{
    /// <summary>
    /// List representing a set of items that have been been paged. Provides a total count of the original set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedEnumerable<T>
    {
        public PagedList(IEnumerable<T> items, int totalCount)
            : base(items)
        {
            TotalCount = totalCount;
        }

        public int TotalCount { get; }
    }
}
