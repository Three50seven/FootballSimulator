using System;

namespace Common.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreDateTimeFormattingAttribute : Attribute
    {
        // Intended to be used on DateTime properties on models that should not be checked/formatted for UTC.
        // By default, all DateTimes should be formatted, but this serves as an option to not.
    }
}
