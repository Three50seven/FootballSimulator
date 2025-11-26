using Humanizer;
using System.Data;

namespace Common.Data
{
    public static class EnumerableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, bool underscore = true) where T : class
        {
            var properties = typeof(T).GetProperties();
            var dataTable = new DataTable();

            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(
                    underscore ? info.Name.Underscore() : info.Name,
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType)
                );
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
