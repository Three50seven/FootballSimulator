using Common.Core.Domain;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class DataSetWriterExtensions
    {
        /// <summary>
        /// Write supplied dataset to file. Stream from writer is copied to file stream based on supplied filename.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string WriteToFile(this IDataSetWriter writer, DataSet data, string fileName)
        {
            using (var stream = writer.Write(data))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
            }

            return fileName;
        }

        /// <summary>
        /// Write supplied dataset to file. Stream from writer is copied to file stream based on supplied filename.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> WriteToFileAsync(this IDataSetWriter writer, DataSet data, string fileName)
        {
            using (var stream = await writer.WriteAsync(data))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }

            return fileName;
        }

        /// <summary>
        /// Write supplied dataset to file. Stream from writer is copied to file stream based on supplied filename. Returns details and stream for the file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileData WriteToData(this IDataSetWriter writer, DataSet data, string fileName)
        {
            using (var stream = (MemoryStream)writer.Write(data))
            {
                return new FileData(stream, fileName);
            }
        }

        /// <summary>
        /// Write supplied dataset to file. Stream from writer is copied to file stream based on supplied filename. Returns details and stream for the file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<FileData> WriteToDataAsync(this IDataSetWriter writer, DataSet data, string fileName)
        {
            using (var stream = (MemoryStream)await writer.WriteAsync(data))
            {
                return new FileData(stream, fileName);
            }
        }
    }
}
