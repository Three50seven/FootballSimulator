using System;

namespace Common.Core
{
    public interface ISerializer
    {
        object Deserialize(string serializedValue, Type type);
        string Serialize(object value);
    }
}
