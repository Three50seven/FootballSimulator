using Common.Core.Annotations;
using Common.Core.Domain;
using System;

namespace Common.Core
{
    /// <summary>
    /// Class level attribute for <see cref="DomainEntity"/> classes to store historical changes on its objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class StoreHistoryAttribute : Attribute
    {
        /// <summary>
        /// Class level attribute for <see cref="DomainEntity"/> classes to store historical changes on its objects.
        /// </summary>
        /// <param name="entityTypeId">Application's entity type identifier.</param>
        /// <param name="includePropChanges">Whether or not individual property changes should be tracked per entity change. Works with <see cref="TrackChangesAttribute"/> properties. Defaults to false.</param>.
        /// <param name="recordAllEvents">
        /// Whether or not all change events to the object should be recorded. If true, the process will record each time object changes. 
        /// If false, the process will be setup to save created and last updated events only.
        /// </param>
        public StoreHistoryAttribute(
            int entityTypeId, 
            bool includePropChanges = false,
            bool recordAllEvents = true)
        {
            EntityTypeId = entityTypeId;            
            IncludePropChanges = includePropChanges;
            RecordAllEvents = recordAllEvents;
        }

        /// <summary>
        /// Application's entity type identifier.
        /// </summary>
        public int EntityTypeId { get; set; }

        /// <summary>
        /// Whether or not individual property changes should be tracked per entity change. Works with <see cref="TrackChangesAttribute"/> properties. Defaults to false.
        /// </summary>
        public bool IncludePropChanges { get; set; }

        /// <summary>
        /// Whether or not all change events to the object should be recorded. If true, the process will record each time object changes. 
        /// If false, the process will be setup to save created and last updated events only.
        /// </summary>
        public bool RecordAllEvents { get; }
    }
}
