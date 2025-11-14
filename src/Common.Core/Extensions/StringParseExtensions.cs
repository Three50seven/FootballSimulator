namespace Common.Core
{
    public static class StringParseExtensions
    {
        //
        // Summary:
        //     Attempt to parse string input as an integer.
        //
        // Parameters:
        //   value:
        //
        //   allowEmpty:
        //     Whether value is allowed to be empty. Returns 0 if true and value is null or
        //     empty.
        //
        //   throwError:
        //     Whether an exception should be thrown if parsing fails or value is empty.
        public static int ParseInteger(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                {
                    return 0;
                }

                if (throwError)
                {
                    throw new ArgumentNullException("value");
                }

                return 0;
            }

            if (!int.TryParse(value, out var result))
            {
                if (throwError)
                {
                    throw new FormatException("String value of " + value + " not correct format for parsing as integer.");
                }

                result = 0;
            }

            return result;
        }

        //
        // Summary:
        //     Attempt to parse string input as a decimal.
        //
        // Parameters:
        //   value:
        //
        //   allowEmpty:
        //     Whether value is allowed to be empty. Returns 0.0M if true and value is null
        //     or empty.
        //
        //   throwError:
        //     Whether an exception should be thrown if parsing fails or value is empty.
        public static decimal ParseDecimal(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                {
                    return 0.0m;
                }

                if (throwError)
                {
                    throw new ArgumentNullException("value");
                }

                return 0.0m;
            }

            if (!decimal.TryParse(value, out var result))
            {
                if (throwError)
                {
                    throw new FormatException("String value of " + value + " not correct format for parsing as decimal.");
                }

                result = 0.0m;
            }

            return result;
        }

        //
        // Summary:
        //     Attempt to parse string input as a double.
        //
        // Parameters:
        //   value:
        //
        //   allowEmpty:
        //     Whether value is allowed to be empty. Returns 0 if true and value is null or
        //     empty.
        //
        //   throwError:
        //     Whether an exception should be thrown if parsing fails or value is empty.
        public static double ParseDouble(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                {
                    return 0.0;
                }

                if (throwError)
                {
                    throw new ArgumentNullException("value");
                }

                return 0.0;
            }

            if (!double.TryParse(value, out var result))
            {
                if (throwError)
                {
                    throw new FormatException("String value of " + value + " not correct format for parsing as double.");
                }

                result = 0.0;
            }

            return result;
        }

        //
        // Summary:
        //     Attempt to parse string input as a bool.
        //
        // Parameters:
        //   value:
        //
        //   allowEmpty:
        //     Whether value is allowed to be empty. Returns false if true and value is null
        //     or empty.
        //
        //   throwError:
        //     Whether an exception should be thrown if parsing fails or value is empty.
        public static bool ParseBool(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                {
                    return false;
                }

                if (throwError)
                {
                    throw new ArgumentNullException("value");
                }

                return false;
            }

            if (!bool.TryParse(value, out var result))
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
                }

                if (throwError)
                {
                    throw new FormatException("String value of " + value + " not correct format for parsing as bool.");
                }

                result = false;
            }

            return result;
        }

        //
        // Summary:
        //     Attempt to parse string input as a long.
        //
        // Parameters:
        //   value:
        //
        //   allowEmpty:
        //     Whether value is allowed to be empty. Returns 0 if true and value is null or
        //     empty.
        //
        //   throwError:
        //     Whether an exception should be thrown if parsing fails or value is empty.
        public static long ParseLong(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                {
                    return 0L;
                }

                if (throwError)
                {
                    throw new ArgumentNullException("value");
                }

                return 0L;
            }

            if (!long.TryParse(value, out var result))
            {
                if (throwError)
                {
                    throw new FormatException("String value of " + value + " not correct format for parsing as long.");
                }

                result = 0L;
            }

            return result;
        }

        //
        // Summary:
        //     Attempt to parse string input as a nullable DateTime.
        //
        // Parameters:
        //   value:
        //
        //   allowEmpty:
        //     Whether value is allowed to be empty. Returns null if true and value is null
        //     or empty.
        //
        //   throwError:
        //     Whether an exception should be thrown if parsing fails or value is empty.
        public static DateTime? ParseDateTime(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                {
                    return null;
                }

                if (throwError)
                {
                    throw new ArgumentNullException("value");
                }

                return null;
            }

            if (!DateTime.TryParse(value, out var result))
            {
                if (throwError)
                {
                    throw new FormatException("String value of " + value + " not correct format for parsing as DateTime.");
                }

                return null;
            }

            return result;
        }

        //
        // Summary:
        //     Attempt to parse string input as a nullable Guid.
        //
        // Parameters:
        //   value:
        //
        //   allowEmpty:
        //     Whether value is allowed to be empty. Returns null if true and value is null
        //     or empty.
        //
        //   throwError:
        //     Whether an exception should be thrown if parsing fails or value is empty.
        public static Guid? ParseGuid(this string value, bool allowEmpty = false, bool throwError = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (allowEmpty)
                {
                    return null;
                }

                if (throwError)
                {
                    throw new ArgumentNullException("value");
                }

                return null;
            }

            if (!Guid.TryParse(value, out var result))
            {
                if (throwError)
                {
                    throw new FormatException("String value of " + value + " not correct format for parsing as Guid.");
                }

                return null;
            }

            return result;
        }
    }
}
