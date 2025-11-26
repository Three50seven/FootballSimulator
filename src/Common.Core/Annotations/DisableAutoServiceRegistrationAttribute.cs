using System;

namespace Common.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DisableAutoServiceRegistrationAttribute : Attribute
    {
    }
}
