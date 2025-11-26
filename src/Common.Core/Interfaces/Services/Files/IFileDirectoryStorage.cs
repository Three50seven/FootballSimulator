using Common.Core.Domain;
using System.Threading.Tasks;

namespace Common.Core.Interfaces
{
    public interface IFileDirectoryStorage
    {
        FileDirectory GetDirectory(string path,
            FileDirectoryIncludeOption directoryIncludes = FileDirectoryIncludeOption.None,
            FilesIncludeOption fileIncludes = FilesIncludeOption.None);

        Task<FileDirectory> GetDirectoryAsync(string path,
            FileDirectoryIncludeOption directoryIncludes = FileDirectoryIncludeOption.None,
            FilesIncludeOption fileIncludes = FilesIncludeOption.None);

        FileDirectory CreateDirectory(string path);
        Task<FileDirectory> CreateDirectoryAsync(string path);
    }
}
