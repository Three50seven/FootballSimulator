using System;

namespace Common.Core.Annotations
{
    /// <summary>
    /// When application is set to track change and when the class for the property
    /// is decorated with <see cref="StoreHistoryAttribute"/>, the changes to this property
    /// will be tracked.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TrackChangesAttribute : Attribute
    {
        public static string DefaultChangeDescriptionFormat = "{0} changed from '{1}' to '{2}'.";
        public static string DefaultAddDescriptionFormat = "{0} set to '{2}'.";

        /// <summary>
        /// Optional friendly name for the property. If not supplied, the property's name will be used.
        /// </summary>
        public string? PropertyName { get; set; }

        /// <summary>
        /// For properties that represent complex types or list of child entities, this value represents 
        /// the property name on the child entity that references back to the parent entity.
        /// Use this value in conjuction with "ParentIdProperty" to define the relationship between the entity and the property entity.
        /// Ex. Person has list of Address that need to be tracked. Person as a List of Addresses, Address has a PersonId, and Person has "Id".
        ///     Place the attribute on the Addresses proprty of Person and specify "PersonId" for this "ForeignKeyProperty" value.
        /// </summary>
        public string? ForeignKeyProperty { get; set; }

        /// <summary>
        /// For properties that represent complex types or list of child entities, this value represents 
        /// the property name on the parent entity that is referenced via the "ForeignKeyProperty" for the child entity.
        /// The default value is "Id" as that is typically the identifier on the parent entity that is referenced by the child entity.
        /// </summary>
        public string? ParentIdProperty { get; set; } = "Id";

        /// <summary>
        /// Type that represents another entity in the system.
        /// This property's value will be used to lookup entity of that type via that entity's identifier and return name.
        /// </summary>
        public Type? LookupType { get; set; }

        /// <summary>
        /// Property on the LookupType entity that should be used as the identifier/PK when looking up the entity.
        /// Defaults to "Id".
        /// </summary>
        public string? LookupPrimaryKeyProperty { get; set; } = "Id";

        /// <summary>
        /// Property on the LookupType entity that represents the description that should be used to represent that entity.
        /// Defaults to "Name".
        /// </summary>
        public string? LookupDescProperty { get; set; } = "Name";

        /// <summary>
        /// Custom string format for the description of the property change event.
        /// Only applies to simple property changes, not add/removing entity changes.
        /// {0} = Property Name, {1} = Old Value, {2} = New Value
        /// </summary>
        public string? ChangeDescriptionFormat { get; set; } = DefaultChangeDescriptionFormat;

        /// <summary>
        /// Custom string format for the description of the property change event when the entity is new (being added).
        /// Only applies to simple property changes, not add/removing entity changes.
        /// {0} = Property Name, {1} = Old Value, {2} = New Value
        /// </summary>
        public string? AddDescriptionFormat { get; set; } = DefaultAddDescriptionFormat;

        public bool IsLookupType => LookupType != null
            && !string.IsNullOrWhiteSpace(LookupPrimaryKeyProperty)
            && !string.IsNullOrWhiteSpace(LookupDescProperty);
    }
}
