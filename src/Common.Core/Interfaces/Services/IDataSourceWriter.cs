using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IDataSourceWriter
    {
        Stream Write<T>(IEnumerable<T> data);
        Task<Stream> WriteAsync<T>(IEnumerable<T> data);
    }
}
