namespace Common.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StoreHistoryAttribute : Attribute
    {
        //
        // Summary:
        //     Application's entity type identifier.
        public int EntityTypeId { get; set; }

        //
        // Summary:
        //     Whether or not individual property changes should be tracked per entity change.
        //     Works with Common.Core.Annotations.TrackChangesAttribute properties. Defaults
        //     to false.
        public bool IncludePropChanges { get; set; }

        //
        // Summary:
        //     Whether or not all change events to the object should be recorded. If true, the
        //     process will record each time object changes. If false, the process will be setup
        //     to save created and last updated events only.
        public bool RecordAllEvents { get; }

        //
        // Summary:
        //     Class level attribute for Common.Core.Domain.DomainEntity classes to store
        //     historical changes on its objects.
        //
        // Parameters:
        //   entityTypeId:
        //     Application's entity type identifier.
        //
        //   includePropChanges:
        //     Whether or not individual property changes should be tracked per entity change.
        //     Works with Common.Core.Annotations.TrackChangesAttribute properties. Defaults
        //     to false.
        //
        //   recordAllEvents:
        //     Whether or not all change events to the object should be recorded. If true, the
        //     process will record each time object changes. If false, the process will be setup
        //     to save created and last updated events only.
        public StoreHistoryAttribute(int entityTypeId, bool includePropChanges = false, bool recordAllEvents = true)
        {
            EntityTypeId = entityTypeId;
            IncludePropChanges = includePropChanges;
            RecordAllEvents = recordAllEvents;
        }
    }
}
