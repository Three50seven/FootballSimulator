using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Common.Core.Annotations;
using Common.Core.Domain;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public static class PropertyEntryExtensions
    {
        /// <summary>
        /// Get the property change values from a property entry.
        /// </summary>
        /// <param name="propertyEntry"></param>
        /// <param name="name">Friendly name for property.</param>
        /// <param name="format">Required format to use for the change description.</param>
        /// <returns></returns>
        public static EntityPropertyChange GetChangeValue(
            this PropertyEntry propertyEntry,
            string name,
            string format = null)
        {
            if (propertyEntry == null)
                return null;

            var oldVal = propertyEntry.OriginalValue?.ToString() ?? string.Empty;
            var newVal = propertyEntry.CurrentValue?.ToString() ?? string.Empty;

            var property = new EntityProperty(
                propertyEntry.Metadata.PropertyInfo.Name,
                propertyEntry.Metadata.PropertyInfo.PropertyType,
                name);

            return new EntityPropertyChange(
                property,
                new EntityChange(string.Format(format ?? "Property: {0} | Old Value: '{1}' | New Value '{2}'", property.FriendlyName, oldVal, newVal), 
                    oldVal, 
                    newVal));
        }

        /// <summary>
        /// Attempt to get property change value based on a given entry set to track changes.
        /// Returns true if changes were discovered, false if no changes.
        /// </summary>
        /// <param name="propertyEntry"></param>
        /// <param name="entityEntry"></param>
        /// <param name="trackChangesAttr"></param>
        /// <param name="propertyChange"></param>
        /// <returns></returns>
        public static bool TryGetTrackingChangeValue(
            this PropertyEntry propertyEntry,
            EntityEntry entityEntry,
            TrackChangesAttribute trackChangesAttr,
            out EntityPropertyChange propertyChange)
        {
            propertyChange = null;

            if (propertyEntry == null || entityEntry == null || trackChangesAttr == null)
                return false;

            // determine the change's description format based on adding new entity or not
            // {0} = Property Name, {1} = Old Value, {2} = New Value
            string descriptionFormat = trackChangesAttr.GetDescriptionFormat(entityEntry.State);

            // convert to change property
            propertyChange = propertyEntry.GetChangeValue(trackChangesAttr.PropertyName, descriptionFormat);

            // see if property value truly changed or new entity (values will be the same when adding new entity)
            if (propertyChange.Change.NewValue != propertyChange.Change.OldValue
                || entityEntry.State == EntityState.Added)
            {
                // see if property is defined as a lookup type
                // this means that it's some identifier for another entity that will need to looked up to get before/after values
                if (trackChangesAttr.IsLookupType)
                {
                    // get old and new values of the property into local variables
                    // this is needed because out params are not permitted in expressions (where clause)
                    string oldValue = propertyChange.Change.OldValue;
                    string newValue = propertyChange.Change.NewValue;

                    var oldValueDesc = entityEntry.Context.Set(trackChangesAttr.LookupType)
                                                  .Where(x => EF.Property<string>(x, trackChangesAttr.LookupPrimaryKeyProperty) == oldValue)
                                                  .Select(x => EF.Property<string>(x, trackChangesAttr.LookupDescProperty))
                                                  .FirstOrDefault();

                    var newValueDesc = entityEntry.Context.Set(trackChangesAttr.LookupType)
                                                  .Where(x => EF.Property<string>(x, trackChangesAttr.LookupPrimaryKeyProperty) == newValue)
                                                  .Select(x => EF.Property<string>(x, trackChangesAttr.LookupDescProperty))
                                                  .FirstOrDefault();

                    // update the change's description to include the new and old value description values using the format
                    propertyChange = new EntityPropertyChange(
                        propertyChange.Property,
                        new EntityChange(string.Format(descriptionFormat, propertyChange.Property.FriendlyName, oldValueDesc, newValueDesc),
                            propertyChange.Change.OldValue,
                            propertyChange.Change.NewValue));
                }

                return true;
            }

            // property is not considered changed
            propertyChange = null;
            return false;
        }
    }
}
