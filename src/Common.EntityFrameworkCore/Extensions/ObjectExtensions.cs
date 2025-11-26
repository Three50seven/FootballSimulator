namespace Common.EntityFrameworkCore
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Translate a given object <paramref name="value"/> to it's SQL string representation.
        /// If <paramref name="value"/> is null or <see cref="string.Empty"/>, "NULL" is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSqlNullableValueInsert(this object value)
        {
            if ((value is string && string.IsNullOrWhiteSpace((string)value)) || value == null)
            {
                return "NULL";
            }
            else
            {
                string? sqlFriendlyString = value.ToString() ?? string.Empty;
                return $"'{sqlFriendlyString.Replace("'", "''")}'";
            }
        }
    }
}
