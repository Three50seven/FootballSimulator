using System.Collections.Generic;

namespace Common.Core
{
    /// <summary>
    /// Enumerable offering a total count of an intended list prior to being paged to the current enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagedEnumerable<T> : IEnumerable<T>
    {
        int TotalCount { get; }
    }
}
