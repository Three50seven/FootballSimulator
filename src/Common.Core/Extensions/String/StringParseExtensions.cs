using System;

namespace Common.Core
{
    public static class StringParseExtensions
    {
        /// <summary>
        /// Attempt to parse string input as an integer.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowEmpty">Whether value is allowed to be empty. Returns 0 if true and value is null or empty.</param>
        /// <param name="throwError">Whether an exception should be thrown if parsing fails or value is empty.</param>
        /// <returns></returns>
        public static int ParseInteger(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                    return 0;
                else if (throwError)
                    throw new ArgumentNullException(nameof(value));
                else
                    return 0;
            }

            if (!int.TryParse(value, out int num))
            {
                if (throwError)
                    throw new FormatException($"String value of {value} not correct format for parsing as integer.");
                else
                    num = 0;
            }

            return num;
        }

        /// <summary>
        /// Attempt to parse string input as a decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowEmpty">Whether value is allowed to be empty. Returns 0.0M if true and value is null or empty.</param>
        /// <param name="throwError">Whether an exception should be thrown if parsing fails or value is empty.</param>
        /// <returns></returns>
        public static decimal ParseDecimal(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                    return 0.0M;
                else if (throwError)
                    throw new ArgumentNullException(nameof(value));
                else
                    return 0.0M;
            }

            if (!decimal.TryParse(value, out decimal num))
            {
                if (throwError)
                    throw new FormatException($"String value of {value} not correct format for parsing as decimal.");
                else
                    num = 0.0M;
            }

            return num;
        }

        /// <summary>
        /// Attempt to parse string input as a double.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowEmpty">Whether value is allowed to be empty. Returns 0 if true and value is null or empty.</param>
        /// <param name="throwError">Whether an exception should be thrown if parsing fails or value is empty.</param>
        /// <returns></returns>
        public static double ParseDouble(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                    return 0;
                else if (throwError)
                    throw new ArgumentNullException(nameof(value));
                else
                    return 0;
            }

            if (!double.TryParse(value, out double num))
            {
                if (throwError)
                    throw new FormatException($"String value of {value} not correct format for parsing as double.");
                else
                    num = 0;
            }

            return num;
        }

        /// <summary>
        /// Attempt to parse string input as a bool.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowEmpty">Whether value is allowed to be empty. Returns false if true and value is null or empty.</param>
        /// <param name="throwError">Whether an exception should be thrown if parsing fails or value is empty.</param>
        /// <returns></returns>
        public static bool ParseBool(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                    return false;
                else if (throwError)
                    throw new ArgumentNullException(nameof(value));
                else
                    return false;
            }

            if (!bool.TryParse(value, out bool booleanValue))
            {
                value = value.ToLower();
                switch (value)
                {
                    case "1":
                    case "yes":
                    case "true":
                    case "t":
                    case "y":
                        return true;
                    case "0":
                    case "no":
                    case "false":
                    case "f":
                    case "n":
                        return false;
                    default:
                        break;
                }

                if (throwError)
                    throw new FormatException($"String value of {value} not correct format for parsing as bool.");
                else
                    booleanValue = false;
            }

            return booleanValue;
        }

        /// <summary>
        /// Attempt to parse string input as a long.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowEmpty">Whether value is allowed to be empty. Returns 0 if true and value is null or empty.</param>
        /// <param name="throwError">Whether an exception should be thrown if parsing fails or value is empty.</param>
        /// <returns></returns>
        public static long ParseLong(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                    return 0;
                else if (throwError)
                    throw new ArgumentNullException(nameof(value));
                else
                    return 0;
            }

            if (!long.TryParse(value, out long num))
            {
                if (throwError)
                    throw new FormatException($"String value of {value} not correct format for parsing as long.");
                else
                    num = 0;
            }

            return num;
        }

        /// <summary>
        /// Attempt to parse string input as a nullable DateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowEmpty">Whether value is allowed to be empty. Returns null if true and value is null or empty.</param>
        /// <param name="throwError">Whether an exception should be thrown if parsing fails or value is empty.</param>
        /// <returns></returns>
        public static DateTime? ParseDateTime(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                    return null;
                else if (throwError)
                    throw new ArgumentNullException(nameof(value));
                else
                    return null;
            }

            if (!DateTime.TryParse(value, out DateTime date))
            {
                if (throwError)
                    throw new FormatException($"String value of {value} not correct format for parsing as DateTime.");
                else
                    return null;
            }

            return date;
        }

        /// <summary>
        /// Attempt to parse string input as a nullable Guid.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowEmpty">Whether value is allowed to be empty. Returns null if true and value is null or empty.</param>
        /// <param name="throwError">Whether an exception should be thrown if parsing fails or value is empty.</param>
        /// <returns></returns>
        public static Guid? ParseGuid(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                    return null;
                else if (throwError)
                    throw new ArgumentNullException(nameof(value));
                else
                    return null;
            }

            if (!Guid.TryParse(value, out Guid guid))
            {
                if (throwError)
                    throw new FormatException($"String value of {value} not correct format for parsing as Guid.");
                else
                    return null;
            }

            return guid;
        }
    }
}
