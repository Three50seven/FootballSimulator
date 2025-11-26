using System;

namespace Common.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowEmptyStringAttribute : Attribute
    {
        // Intended to be used on string properties that dictate the given string is allowed to be empty and should not be formatted to null.
        // Use this sparingly and only when required; string properties should be properly trimmed and nullified before being persisted.   
    }
}
