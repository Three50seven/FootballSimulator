using Common.Core.Domain;
using System;
using System.Collections.Generic;

namespace Common.Core.Validation
{
    public sealed class Guard
    {
        public static readonly Guard Instance = new Guard();

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if object <paramref name="value"/> is null.
        /// If <paramref name="value"/> is a <see cref="string"></see>, <see cref="string.IsNullOrWhiteSpace(string)"/> is called to determine null.
        /// </summary>
        /// <typeparam name="T">Any class type.</typeparam>
        /// <param name="value">Object to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void IsNotNull<T>(T value, string paramName) where T : class
        {
            if (value == null || (value is string && string.IsNullOrWhiteSpace(value as string)))
                throw new ArgumentNullException(paramName, $"Null value of type {typeof(T)}.");
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if object <paramref name="value"/> is null.
        /// </summary>
        /// <typeparam name="T">Any nullable struct type.</typeparam>
        /// <param name="value">Object to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void IsNotNullValueType<T>(Nullable<T> value, string paramName) where T : struct
        {
            if (value == null)
                throw new ArgumentNullException(paramName, $"Null value of type {typeof(T)}.");
        }

        /// <summary>
        /// Throws a <see cref="ArgumentException"/> if object <paramref name="value"/> equals the default operator of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Any class type.</typeparam>
        /// <param name="value">Object to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsNotDefault<T>(T value, string paramName) where T : class
        {
            if (value == default(T))
                throw new ArgumentException($"{paramName} of type {typeof(T)} cannot be default value {value}.");
        }

        /// <summary>
        /// Throws a <see cref="ArgumentException"/> if object <paramref name="value"/> equals the default operator of <typeparamref name="T"/>.
        /// Equals operator <see cref="object.Equals(object)"/> is used as the determinant.
        /// </summary>
        /// <typeparam name="T">Any struct type.</typeparam>
        /// <param name="value">Object to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsNotDefaultValueType<T>(T value, string paramName) where T : struct
        {
            if (value.Equals(default(T)))
                throw new ArgumentException($"{paramName} of type {typeof(T)} cannot be default value {value}");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if numeric value of <paramref name="value"/> is less than zero.
        /// </summary>
        /// <param name="value">Numeric value.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="value"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void IsPositive(long value, string paramName)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(paramName, $"Must be positive, but was {value}.");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if numeric value of <paramref name="value"/> is greater than zero.
        /// </summary>
        /// <param name="value">Numeric value.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="value"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void IsNegative(long value, string paramName)
        {
            if (value > 0)
                throw new ArgumentOutOfRangeException(paramName, $"Must be negative, but was {value}.");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if numeric value of <paramref name="value"/> is less than or equal to zero.
        /// </summary>
        /// <param name="value">Numeric value.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="value"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void IsGreaterThanZero(int value, string paramName)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(paramName, $"Must be greater than zero, but was {value}.");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if numeric value of <paramref name="value"/> is greater than or equal to zero.
        /// </summary>
        /// <param name="value">Numeric value.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="value"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void IsLessThanZero(int value, string paramName)
        {
            if (value >= 0)
                throw new ArgumentOutOfRangeException(paramName, $"Must be less than zero, but was {value}.");
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if <paramref name="startDate"/> comes after <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new InvalidOperationException($"Starting date ({startDate}) cannot be greater than ending date ({endDate}) in range calculations.");
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="date"/> does not contain a valid value.
        /// Operator <see cref="NullableExtensions.HasValue(DateTime)"/> is used to evaluate.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="date"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void DateIsSet(DateTime date, string paramName)
        {
            if (!date.HasValue())
                throw new ArgumentNullException(paramName, $"DateTime value is not set. Value = {date}");
        }
        
        /// <summary>
        /// Throws <see cref="InvalidOperationException"></see> if values are considered equal under <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <typeparam name="T">Any class type.</typeparam>
        /// <param name="value"></param>
        /// <param name="comparingValue"></param>
        /// <param name="validationMessage">Optional validation message to be included in the exception message.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AreNotEqual<T>(T value, T comparingValue, string validationMessage = "Values are not allowed to be equal to each other.")
        {
            if (value.Equals(comparingValue))
                throw new InvalidOperationException($"{validationMessage} Type:{typeof(T)} {nameof(value)}:{value} {nameof(comparingValue)}:{comparingValue}");
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"></see> if values are not considered equal under <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <typeparam name="T">Any class type.</typeparam>
        /// <param name="value"></param>
        /// <param name="comparingValue"></param>
        /// <param name="validationMessage">Optional validation message to be included in the exception message.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AreEqual<T>(T value, T comparingValue, string validationMessage = "Values must be equal to each other.")
        {
            if (!value.Equals(comparingValue))
                throw new InvalidOperationException($"{validationMessage} Type:{typeof(T)} {nameof(value)}:{value} {nameof(comparingValue)}:{comparingValue}");
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if entity object <paramref name="item"/> is considered new under <see cref="IEntity{TId}.IsNew"/>.
        /// </summary>
        /// <typeparam name="T">Class entity type.</typeparam>
        /// <typeparam name="K">Key type for entity.</typeparam>
        /// <param name="item">Entity to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="item"/>.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void IsNotNew<T, K>(T item, string paramName) where T : IEntity<K>
        {
            if (item != null && item.IsNew)
                throw new InvalidOperationException($"Entity of type {item.GetType()} cannot be new in this context for parameter: {paramName}.");
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if entity object <paramref name="item"/> is considered new under <see cref="IEntity{TId}.IsNew"/>.
        /// </summary>
        /// <typeparam name="T">Class entity type.</typeparam>
        /// <param name="item">Entity to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="item"/>.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void IsNotNew<T>(T item, string paramName) where T : IEntity<int>
        {
            IsNotNew<T, int>(item, paramName);
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if entity object <paramref name="item"/> is not considered new under <see cref="IEntity{TId}.IsNew"/>.
        /// </summary>
        /// <typeparam name="T">Class entity type.</typeparam>
        /// <typeparam name="K">Key type for entity.</typeparam>
        /// <param name="item">Entity to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="item"/>.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void IsNew<T, K>(T item, string paramName) where T : IEntity<K>
        {
            if (item != null && !item.IsNew)
                throw new InvalidOperationException($"Entity of type {item.GetType()} must only be new in this context for parameter: {paramName}.");
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if entity object <paramref name="item"/> is not considered new under <see cref="IEntity{TId}.IsNew"/>.
        /// </summary>
        /// <typeparam name="T">Class entity type.</typeparam>
        /// <param name="item">Entity to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="item"/>.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void IsNew<T>(T item, string paramName) where T : IEntity<int>
        {
            IsNew<T, int>(item, paramName);
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if list of items is empty or null.
        /// Calls <see cref="EnumerableExtensions.HasItems{T}(IEnumerable{T})"></see> to evaluate.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="items">List of items to evaluate.</param>
        /// <param name="paramName">String parameter/identifier for the object <paramref name="items"/>.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void IsNotNullOrEmptyList<T>(IEnumerable<T> items, string paramName)
        {
            if (!items.HasItems())
                throw new InvalidOperationException($"IEnumerable of type {typeof(T).FullName} cannot be empty or null for parameter: {paramName}.");
        }
    }
}
