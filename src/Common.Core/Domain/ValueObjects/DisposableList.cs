using System;
using System.Collections.Generic;

namespace Common.Core.Domain
{
    /// <summary>
    /// Simple generic list class for disposable items that also itself is disposable.
    /// When disposing this list, all items of the list will be disposed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisposableList<T> : List<T>, IDisposable where T : IDisposable
    {
        public DisposableList(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public void Dispose()
        {
            foreach (var item in this)
            {
                item.Dispose();
            }
        }
    }
}
