using System;

namespace Common.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowZeroAttribute : Attribute
    {
        // Intended to be used on nullable or non-nullable int properties that dictate the given int is allowed to be zero.
        // Use this sparingly and only when required; typically, nullable int properties with a value of zero should be properly adjusted to null before being persisted.   
    }
}
