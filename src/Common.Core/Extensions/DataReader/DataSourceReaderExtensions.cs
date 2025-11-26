using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class DataSourceReaderExtensions
    {
        /// <summary>
        /// Open and ready supplied file path into list of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<T> Read<T>(this IDataSourceReader reader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return reader.Read<T>(stream);
            }
        }

        /// <summary>
        /// Open and ready supplied file path into list of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> ReadAsync<T>(this IDataSourceReader reader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return await reader.ReadAsync<T>(stream);
            }
        }
    }
}
