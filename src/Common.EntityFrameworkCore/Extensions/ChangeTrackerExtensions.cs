using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public static class ChangeTrackerExtensions
    {
        /// <summary>
        /// Clear all entries in the ChangeTracker <paramref name="changeTracker"/> by setting state to <see cref="EntityState.Detached"/>.
        /// </summary>
        /// <param name="changeTracker">Active change tracker. Typically from <see cref="DbContext.ChangeTracker"/>.</param>
        /// <returns></returns>
        public static ChangeTracker ClearEntries(this ChangeTracker changeTracker)
        {
            if (changeTracker == null)
                return null;

            foreach (var entry in changeTracker.Entries()?.Where(e => e.Entity != null))
            {
                entry.State = EntityState.Detached;
            }

            return changeTracker;
        }
    }
}
