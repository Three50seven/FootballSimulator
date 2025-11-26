using System;

namespace Common.Core.Services
{
    public class ValueParser : IValueParser
    {
        public object Parse(string value, Type type)
        {
            var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            bool allowNull = false;
            if (nullableUnderlyingType != null)
            {
                type = nullableUnderlyingType;
                allowNull = true;
            }

            if (type == typeof(string))
                return value;
            else if (type == typeof(bool))
                return value.ParseBool(allowEmpty: allowNull);
            else if (type == typeof(int))
                return value.ParseInteger(allowEmpty: allowNull);
            else if (type == typeof(decimal))
                return value.ParseDecimal(allowEmpty: allowNull);
            else if (type == typeof(double))
                return value.ParseDouble(allowEmpty: allowNull);
            else if (type == typeof(DateTime))
                return value.ParseDateTime(allowEmpty: allowNull);
            else if (type == typeof(Guid))
                return value.ParseGuid(allowEmpty: allowNull);
            else
                throw new InvalidOperationException($"Type {type.FullName} is not a valid type to Parse using {GetType().FullName}.");
        }

        public bool TryParse(string value, Type type, out object parsedValue)
        {
            parsedValue = default;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            if (nullableUnderlyingType != null)
                type = nullableUnderlyingType;

            if (type == typeof(string))
            {
                parsedValue = value;
                return true;
            }
            else if (type == typeof(bool))
            {
                bool boolValue;
                if (bool.TryParse(value, out boolValue))
                {
                    parsedValue = boolValue;
                    return true;
                }
            }
            else if (type == typeof(int))
            {
                int intValue;
                if (int.TryParse(value, out intValue))
                {
                    parsedValue = intValue;
                    return true;
                }
            }
            else if (type == typeof(decimal))
            {
                decimal decimalValue;
                if (decimal.TryParse(value, out decimalValue))
                {
                    parsedValue = decimalValue;
                    return true;
                }
            }
            else if (type == typeof(double))
            {
                double doubleValue;
                if (double.TryParse(value, out doubleValue))
                {
                    parsedValue = doubleValue;
                    return true;
                }
            }
            else if (type == typeof(DateTime))
            {
                DateTime dateValue;
                if (DateTime.TryParse(value, out dateValue))
                {
                    parsedValue = dateValue;
                    return true;
                }
            }
            else if (type == typeof(Guid))
            {
                Guid guidValue;
                if (Guid.TryParse(value, out guidValue))
                {
                    parsedValue = guidValue;
                    return true;
                }
            }

            return false;
        }
    }
}
