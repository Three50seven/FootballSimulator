using Common.Core.Domain;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IFileSourceRepository
    {
        Task<FileSource> GetByPathAsync(string path);
        Task DeleteAsync(string path);
        Task<bool> ExistsAsync(string path);
        Task AddOrUpdateAsync(FileSource file);
    }
}
