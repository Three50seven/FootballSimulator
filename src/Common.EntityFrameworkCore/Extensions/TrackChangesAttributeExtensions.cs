using Microsoft.EntityFrameworkCore;
using Common.Core.Annotations;

namespace Common.EntityFrameworkCore
{
    public static class TrackChangesAttributeExtensions
    {
        /// <summary>
        /// Determine the description format based on an entity's state.
        /// State of <see cref="EntityState.Added"/> will return <see cref="TrackChangesAttribute.AddDescriptionFormat"/>.
        /// Any other state returns <see cref="TrackChangesAttribute.ChangeDescriptionFormat"/>.
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetDescriptionFormat(this TrackChangesAttribute attr, EntityState state)
        {
            if (attr == null)
                return null;

            return state == EntityState.Added ? attr.AddDescriptionFormat : attr.ChangeDescriptionFormat;
        }
    }
}
