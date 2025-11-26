using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IDataSourceReader
    {
        IEnumerable<T> Read<T>(Stream stream);
        Task<IEnumerable<T>> ReadAsync<T>(Stream stream);
    }
}
