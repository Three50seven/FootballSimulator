using Common.Core.Domain;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IFileStorage
    {
        bool Exists(string path);
        Task<bool> ExistsAsync(string path);

        FileReference Save(Stream stream, string path, bool overwrite = false);
        Task<FileReference> SaveAsync(Stream stream, string path, bool overwrite = false);

        FileReference Save(string content, string path, bool overwrite = false);
        Task<FileReference> SaveAsync(string content, string path, bool overwrite = false);

        Stream Load(string path);
        Task<Stream> LoadAsync(string path);

        string ReadContent(string path);
        Task<string> ReadContentAsync(string path);

        void Delete(string path);
        Task DeleteAsync(string path);
    }
}