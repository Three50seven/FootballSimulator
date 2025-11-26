using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IDataSetWriter
    {
        Stream Write(DataSet data);
        Task<Stream> WriteAsync(DataSet data);
    }
}
