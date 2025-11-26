using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class DataSetReaderExtensions
    {
        /// <summary>
        /// Open and read supplied file path into a dataset.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataSet Read(this IDataSetReader reader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return reader.Read(stream);
            }
        }

        /// <summary>
        /// Open and read supplied file path into a dataset.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<DataSet> ReadAsync(this IDataSetReader reader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return await reader.ReadAsync(stream);
            }
        }
    }
}
