using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Common.Core;
using Common.Core.Annotations;
using Common.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.EntityFrameworkCore
{
    public static class EntityEntryExtensions
    {
        /// <summary>
        /// Get applicable tracking information like value and description to record when tracking changes on the entry (added or deleted).
        /// <see cref="ITrackableEntity.GetTrackingInfo"/> will be used if entry's entity inherits the interface.
        /// Otherwise, ToSting is used for description and entity's Id is returend as value.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="loadNavigations">Where or not EFCore should load any unloaded navigation itmes prior.</param>
        /// <returns></returns>
        public static TrackableInfo GetTrackingInfo(this EntityEntry entry, bool loadNavigations = true)
        {
            if (entry == null || entry.Entity == null)
                return null;

            if (loadNavigations)
                LoadNavigations(entry);

            if (entry.Entity is ITrackableEntity trackableEntity)
            {
                return trackableEntity.GetTrackingInfo();
            }
            else
            {
                var desc = entry.Entity.ToString();
                var value = (entry.Entity as IEntity<int>)?.Id.ToString() ?? (entry.Entity as IEntity<long>)?.Id.ToString() ?? desc;

                return new TrackableInfo(value, desc);
            }
        }

        /// <summary>
        /// Load any navigations on a given entity that have not been loaded.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static EntityEntry LoadNavigations(EntityEntry entry)
        {
            if (entry == null)
                return null;

            // include in navigation/reference entities on the entity prior to getting tracking info
            // NOTE: this does not seem to load some reference entities
            foreach (var navigation in entry.Navigations)
            {
                if (navigation.CurrentValue == null)
                    navigation.IsLoaded = false;

                if (!navigation.IsLoaded)
                    navigation.Load();
            }

            return entry;
        }

        /// <summary>
        /// Assess the given entry for all properties, references, and collections that have changed.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="allEntries">All entries from change tracker.</param>
        /// <returns></returns>
        public static IEnumerable<EntityPropertyChange> GetAllTrackableChanges(
            this EntityEntry entry,
            IEnumerable<EntityEntry> allEntries)
        {
            var changes = new List<EntityPropertyChange>();
            BuildNestedPropertyChanges(changes, entry, allEntries);
            return changes;
        }

        private static void BuildNestedPropertyChanges(
            IList<EntityPropertyChange> changes,
            EntityEntry entry,
            IEnumerable<EntityEntry> allEntries)
        {
            // get all accessible properties for tracking changes
            var entityProps = entry.Entity.GetType().GetProperties().Where(p => p.GetSetMethod(true) != null);

            foreach (var prop in entityProps)
            {
                var trackChangesAttr = prop.GetCustomAttribute<TrackChangesAttribute>(inherit: true);
                if (trackChangesAttr == null)
                    continue;

                var entityMember = entry.Members.Where(m => m.Metadata.PropertyInfo == prop).FirstOrDefault();
                if (entityMember == null)
                    continue;

                // when property entity and is modified or entity is new (member not considered modified when new entry)
                if (entityMember is PropertyEntry && (entityMember.IsModified || entry.State == EntityState.Added))
                {
                    // ** Simple Property **

                    if ((entityMember as PropertyEntry).TryGetTrackingChangeValue(entry, trackChangesAttr, out EntityPropertyChange change))
                        changes.Add(change);
                }
                else if (entityMember is ReferenceEntry)
                {
                    // ** Complex Referenced Property **

                    var referenceEntry = entityMember as ReferenceEntry;
                    var entityProperty = referenceEntry.ToEntityProperty(trackChangesAttr.PropertyName);

                    if (referenceEntry.TargetEntry == null)
                    {
                        // if reference is found as a deleted entry, added deleted change
                        if (allEntries.Any(e => e.State == EntityState.Deleted
                                                && e.Entity.GetType() == referenceEntry.Metadata.ClrType
                                                && e.Metadata == referenceEntry.Metadata.TargetEntityType))
                        {
                            changes.Add(new EntityPropertyChange(
                                entityProperty,
                                new EntityChange($"Removed {trackChangesAttr.PropertyName}", "", "")));
                        }
                    }
                    else
                    {
                        // determine change to add using the entries state
                        switch (referenceEntry.TargetEntry.State)
                        {
                            case EntityState.Deleted:
                                changes.Add(new EntityPropertyChange(
                                        entityProperty,
                                        new EntityChange($"Removed {trackChangesAttr.PropertyName}", "", "")));
                                break;
                            case EntityState.Modified:
                                changes.Add(new EntityPropertyChange(
                                        entityProperty,
                                        new EntityChange($"Modifed {trackChangesAttr.PropertyName}", "", "")));

                                BuildNestedPropertyChanges(changes, referenceEntry.TargetEntry, allEntries);
                                break;
                            case EntityState.Added:
                                changes.Add(new EntityPropertyChange(
                                        entityProperty,
                                        new EntityChange($"Added {trackChangesAttr.PropertyName}", "", "")));
                                break;
                            default:
                                break;
                        }
                    }

                }
                else if (entityMember is CollectionEntry)
                {
                    // ** Collection Property **

                    var collectionEntry = entityMember as CollectionEntry;

                    // check each item in the collection to see if it was added or changed
                    foreach (var collectionItem in collectionEntry.CurrentValue)
                    {
                        var collectionItemEntry = collectionEntry.FindEntry(collectionItem);
                        if (collectionItemEntry != null)
                        {
                            switch (collectionItemEntry.State)
                            {
                                case EntityState.Added:
                                    var trackingInfo = collectionItemEntry.GetTrackingInfo();

                                    changes.Add(new EntityPropertyChange(
                                        new EntityProperty(entityMember.Metadata.PropertyInfo.Name, collectionItem.GetType(), trackChangesAttr.PropertyName),
                                        new EntityChange($"Added {trackChangesAttr.PropertyName} - {trackingInfo.Description}", "", trackingInfo.Value)));

                                    BuildNestedPropertyChanges(changes, collectionItemEntry, allEntries);
                                    break;
                                case EntityState.Modified:
                                case EntityState.Unchanged:
                                    BuildNestedPropertyChanges(changes, collectionItemEntry, allEntries);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // check for any items in the deleted entries with the collection item type in order to record the delete change
                    // must have the FK and parent id prop names set on the tracking attribute
                    if (!string.IsNullOrWhiteSpace(trackChangesAttr.ForeignKeyProperty)
                        && !string.IsNullOrWhiteSpace(trackChangesAttr.ParentIdProperty))
                    {
                        foreach (var deletedEntry in allEntries.Where(e => e.State == EntityState.Deleted
                            && e.Entity.GetType() == collectionEntry.Metadata.TargetEntityType.ClrType))
                        {
                            // use reflection to find associated properties
                            var fkProperty = deletedEntry.Entity.Property(trackChangesAttr.ForeignKeyProperty);
                            var pkProperty = collectionEntry.EntityEntry.Entity.Property(trackChangesAttr.ParentIdProperty);

                            if (fkProperty != null && pkProperty != null)
                            {
                                var fk = fkProperty.GetValue(deletedEntry.Entity);
                                var pk = pkProperty.GetValue(collectionEntry.EntityEntry.Entity);

                                // if the FK and the PK (parent identifier property) match,
                                // that means the deleted entry is indeed a item that was removed from the collection
                                if (fk != null && pk != null && fk.Equals(pk))
                                {
                                    var trackingInfo = deletedEntry.GetTrackingInfo();

                                    changes.Add(new EntityPropertyChange(
                                        new EntityProperty(collectionEntry.Metadata.PropertyInfo.Name, deletedEntry.Entity.GetType(), trackChangesAttr.PropertyName),
                                        new EntityChange($"Removed {trackChangesAttr.PropertyName} - {trackingInfo.Description}", trackingInfo.Value, "")));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
