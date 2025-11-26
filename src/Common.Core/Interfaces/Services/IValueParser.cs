using System;

namespace Common.Core
{
    public interface IValueParser
    {
        object Parse(string value, Type type);
        bool TryParse(string value, Type type, out object parsedValue);
    }
}
