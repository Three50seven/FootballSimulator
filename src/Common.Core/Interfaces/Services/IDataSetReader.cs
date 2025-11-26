using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IDataSetReader
    {
        DataSet Read(Stream stream);
        Task<DataSet> ReadAsync(Stream stream);
    }
}
