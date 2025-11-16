namespace Common.Core
{

    //
    // Summary:
    //     When application is set to track change and when the class for the property is
    //     decorated with Common.Core.StoreHistoryAttribute, the changes to this property
    //     will be tracked.
    [AttributeUsage(AttributeTargets.Property)]
    public class TrackChangesAttribute : Attribute
    {
        public static string DefaultChangeDescriptionFormat = "{0} changed from '{1}' to '{2}'.";

        public static string DefaultAddDescriptionFormat = "{0} set to '{2}'.";

        //
        // Summary:
        //     Optional friendly name for the property. If not supplied, the property's name
        //     will be used.
        public string? PropertyName { get; set; }

        //
        // Summary:
        //     For properties that represent complex types or list of child entities, this value
        //     represents the property name on the child entity that references back to the
        //     parent entity. Use this value in conjuction with "ParentIdProperty" to define
        //     the relationship between the entity and the property entity. Ex. Person has list
        //     of Address that need to be tracked. Person as a List of Addresses, Address has
        //     a PersonId, and Person has "Id". Place the attribute on the Addresses proprty
        //     of Person and specify "PersonId" for this "ForeignKeyProperty" value.
        public string? ForeignKeyProperty { get; set; }

        //
        // Summary:
        //     For properties that represent complex types or list of child entities, this value
        //     represents the property name on the parent entity that is referenced via the
        //     "ForeignKeyProperty" for the child entity. The default value is "Id" as that
        //     is typically the identifier on the parent entity that is referenced by the child
        //     entity.
        public string? ParentIdProperty { get; set; } = "Id";


        //
        // Summary:
        //     Type that represents another entity in the system. This property's value will
        //     be used to lookup entity of that type via that entity's identifier and return
        //     name.
        public Type? LookupType { get; set; }

        //
        // Summary:
        //     Property on the LookupType entity that should be used as the identifier/PK when
        //     looking up the entity. Defaults to "Id".
        public string LookupPrimaryKeyProperty { get; set; } = "Id";


        //
        // Summary:
        //     Property on the LookupType entity that represents the description that should
        //     be used to represent that entity. Defaults to "Name".
        public string LookupDescProperty { get; set; } = "Name";


        //
        // Summary:
        //     Custom string format for the description of the property change event. Only applies
        //     to simple property changes, not add/removing entity changes. {0} = Property Name,
        //     {1} = Old Value, {2} = New Value
        public string ChangeDescriptionFormat { get; set; } = DefaultChangeDescriptionFormat;


        //
        // Summary:
        //     Custom string format for the description of the property change event when the
        //     entity is new (being added). Only applies to simple property changes, not add/removing
        //     entity changes. {0} = Property Name, {1} = Old Value, {2} = New Value
        public string AddDescriptionFormat { get; set; } = DefaultAddDescriptionFormat;


        public bool IsLookupType => LookupType != null && !string.IsNullOrWhiteSpace(LookupPrimaryKeyProperty) && !string.IsNullOrWhiteSpace(LookupDescProperty);
    }
}
