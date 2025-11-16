using Common.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Validation
{
    public sealed class Guard
    {
        public static readonly Guard Instance = new Guard();

        //
        // Summary:
        //     Throws System.ArgumentNullException if object value is null. If value is a System.String,
        //     System.String.IsNullOrWhiteSpace(System.String) is called to determine null.
        //
        //
        // Parameters:
        //   value:
        //     Object to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object value.
        //
        // Type parameters:
        //   T:
        //     Any class type.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        public static void IsNotNull<T>(T value, string paramName) where T : class
        {
            if (value == null || (value is string && string.IsNullOrWhiteSpace(value as string)))
            {
                throw new ArgumentNullException(paramName, $"Null value of type {typeof(T)}.");
            }
        }

        //
        // Summary:
        //     Throws System.ArgumentNullException if object value is null.
        //
        // Parameters:
        //   value:
        //     Object to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object value.
        //
        // Type parameters:
        //   T:
        //     Any nullable struct type.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        public static void IsNotNullValueType<T>(T? value, string paramName) where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(paramName, $"Null value of type {typeof(T)}.");
            }
        }

        //
        // Summary:
        //     Throws a System.ArgumentException if object value equals the default operator
        //     of T.
        //
        // Parameters:
        //   value:
        //     Object to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object value.
        //
        // Type parameters:
        //   T:
        //     Any class type.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        public static void IsNotDefault<T>(T value, string paramName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentException($"{paramName} of type {typeof(T)} cannot be default value {value}.");
            }
        }

        //
        // Summary:
        //     Throws a System.ArgumentException if object value equals the default operator
        //     of T. Equals operator System.Object.Equals(System.Object) is used as the determinant.
        //
        //
        // Parameters:
        //   value:
        //     Object to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object value.
        //
        // Type parameters:
        //   T:
        //     Any struct type.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        public static void IsNotDefaultValueType<T>(T value, string paramName) where T : struct
        {
            if (value.Equals(default(T)))
            {
                throw new ArgumentException($"{paramName} of type {typeof(T)} cannot be default value {value}");
            }
        }

        //
        // Summary:
        //     Throws System.ArgumentOutOfRangeException if numeric value of value is less than
        //     zero.
        //
        // Parameters:
        //   value:
        //     Numeric value.
        //
        //   paramName:
        //     String parameter/identifier for the object value.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        public static void IsPositive(long value, string paramName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Must be positive, but was {value}.");
            }
        }

        //
        // Summary:
        //     Throws System.ArgumentOutOfRangeException if numeric value of value is greater
        //     than zero.
        //
        // Parameters:
        //   value:
        //     Numeric value.
        //
        //   paramName:
        //     String parameter/identifier for the object value.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        public static void IsNegative(long value, string paramName)
        {
            if (value > 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Must be negative, but was {value}.");
            }
        }

        //
        // Summary:
        //     Throws System.ArgumentOutOfRangeException if numeric value of value is less than
        //     or equal to zero.
        //
        // Parameters:
        //   value:
        //     Numeric value.
        //
        //   paramName:
        //     String parameter/identifier for the object value.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        public static void IsGreaterThanZero(int value, string paramName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Must be greater than zero, but was {value}.");
            }
        }

        //
        // Summary:
        //     Throws System.ArgumentOutOfRangeException if numeric value of value is greater
        //     than or equal to zero.
        //
        // Parameters:
        //   value:
        //     Numeric value.
        //
        //   paramName:
        //     String parameter/identifier for the object value.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        public static void IsLessThanZero(int value, string paramName)
        {
            if (value >= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Must be less than zero, but was {value}.");
            }
        }

        //
        // Summary:
        //     Throws System.InvalidOperationException if startDate comes after endDate.
        //
        // Parameters:
        //   startDate:
        //
        //   endDate:
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        public static void IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidOperationException($"Starting date ({startDate}) cannot be greater than ending date ({endDate}) in range calculations.");
            }
        }

        //
        // Summary:
        //     Throws System.ArgumentNullException if date does not contain a valid value. Operator
        //     Common.Core.NullableExtensions.HasValue(System.DateTime) is used to evaluate.
        //
        //
        // Parameters:
        //   date:
        //
        //   paramName:
        //     String parameter/identifier for the object date.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        public static void DateIsSet(DateTime date, string paramName)
        {
            if (!date.HasValue())
            {
                throw new ArgumentNullException(paramName, $"DateTime value is not set. Value = {date}");
            }
        }

        //
        // Summary:
        //     Throws System.InvalidOperationException if values are considered equal under
        //     System.Object.Equals(System.Object).
        //
        // Parameters:
        //   value:
        //
        //   comparingValue:
        //
        //   validationMessage:
        //     Optional validation message to be included in the exception message.
        //
        // Type parameters:
        //   T:
        //     Any class type.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        public static void AreNotEqual<T>(T value, T comparingValue, string validationMessage = "Values are not allowed to be equal to each other.")
        {
            if (value is null && comparingValue is null)
            {
                throw new InvalidOperationException($"{validationMessage} Type:{typeof(T)} value:{value} comparingValue:{comparingValue}");
            }
            if (value is not null && value.Equals(comparingValue))
            {
                throw new InvalidOperationException($"{validationMessage} Type:{typeof(T)} value:{value} comparingValue:{comparingValue}");
            }
        }

        //
        // Summary:
        //     Throws System.InvalidOperationException if values are not considered equal under
        //     System.Object.Equals(System.Object).
        //
        // Parameters:
        //   value:
        //
        //   comparingValue:
        //
        //   validationMessage:
        //     Optional validation message to be included in the exception message.
        //
        // Type parameters:
        //   T:
        //     Any class type.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        public static void AreEqual<T>(T value, T comparingValue, string validationMessage = "Values must be equal to each other.")
        {
            if (value is null && comparingValue is null)
            {
                return;
            }
            if (value is null || !value.Equals(comparingValue))
            {
                throw new InvalidOperationException($"{validationMessage} Type:{typeof(T)} value:{value} comparingValue:{comparingValue}");
            }
        }

        //
        // Summary:
        //     Throws System.InvalidOperationException if entity object item is considered new
        //     under Common.Core.Domain.IEntity`1.IsNew.
        //
        // Parameters:
        //   item:
        //     Entity to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object item.
        //
        // Type parameters:
        //   T:
        //     Class entity type.
        //
        //   K:
        //     Key type for entity.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        public static void IsNotNew<T, K>(T item, string paramName) where T : IEntity<K>
        {
            if (item != null && item.IsNew)
            {
                throw new InvalidOperationException($"Entity of type {item.GetType()} cannot be new in this context for parameter: {paramName}.");
            }
        }

        //
        // Summary:
        //     Throws System.InvalidOperationException if entity object item is considered new
        //     under Common.Core.Domain.IEntity`1.IsNew.
        //
        // Parameters:
        //   item:
        //     Entity to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object item.
        //
        // Type parameters:
        //   T:
        //     Class entity type.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        public static void IsNotNew<T>(T item, string paramName) where T : IEntity<int>
        {
            IsNotNew<T, int>(item, paramName);
        }

        //
        // Summary:
        //     Throws System.InvalidOperationException if entity object item is not considered
        //     new under Common.Core.Domain.IEntity`1.IsNew.
        //
        // Parameters:
        //   item:
        //     Entity to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object item.
        //
        // Type parameters:
        //   T:
        //     Class entity type.
        //
        //   K:
        //     Key type for entity.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        public static void IsNew<T, K>(T item, string paramName) where T : IEntity<K>
        {
            if (item != null && !item.IsNew)
            {
                throw new InvalidOperationException($"Entity of type {item.GetType()} must only be new in this context for parameter: {paramName}.");
            }
        }

        //
        // Summary:
        //     Throws System.InvalidOperationException if entity object item is not considered
        //     new under Common.Core.Domain.IEntity`1.IsNew.
        //
        // Parameters:
        //   item:
        //     Entity to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object item.
        //
        // Type parameters:
        //   T:
        //     Class entity type.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        public static void IsNew<T>(T item, string paramName) where T : IEntity<int>
        {
            IsNew<T, int>(item, paramName);
        }

        //
        // Summary:
        //     Throws System.InvalidOperationException if list of items is empty or null. Calls
        //     Common.Core.EnumerableExtensions.HasItems``1(System.Collections.Generic.IEnumerable{``0})
        //     to evaluate.
        //
        // Parameters:
        //   items:
        //     List of items to evaluate.
        //
        //   paramName:
        //     String parameter/identifier for the object items.
        //
        // Type parameters:
        //   T:
        //     Any type.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        public static void IsNotNullOrEmptyList<T>(IEnumerable<T> items, string paramName)
        {
            if (!items.HasItems())
            {
                throw new InvalidOperationException($"IEnumerable of type {typeof(T).FullName} cannot be empty or null for parameter: {paramName}.");
            }
        }
    }
}
