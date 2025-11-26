using Common.Core.Domain;
using Common.Core.Interfaces;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// File storage and directory/folder storage where files are stored on local file system.
    /// </summary>
    public class LocalSystemFileStorage : IFileStorage, IFileDirectoryStorage
    {
        private readonly IFileSystemPathResolver _fileSystemPathResolver;
        private readonly FileStorageSettings _fileStorageSettings;

        public LocalSystemFileStorage(
            IFileSystemPathResolver fileSystemPathResolver,
            FileStorageSettings fileStorageSettings)
        {
            _fileSystemPathResolver = fileSystemPathResolver;
            _fileStorageSettings = fileStorageSettings;
        }

        private FileStream BuildFileStream(string path, bool async, bool write)
        {
            return new FileStream(path,
                write ? FileMode.Create : FileMode.Open,
                write ? FileAccess.Write : FileAccess.Read,
                FileShare.None,
                bufferSize: _fileStorageSettings.StreamBufferSize,
                useAsync: async);
        }

        private string ResolveFullPath(string relativePath, bool canBeDirectory = false)
        {
            return _fileSystemPathResolver.ResolveFullPath(relativePath, canBeDirectory);
        }
       
        private string PreparePathForSave(string path, bool overwrite)
        {
            path = ResolveFullPath(path);

            if (_fileStorageSettings.CreateDirectories)
                CreateLocalDirectory(path);

            if (File.Exists(path) && (_fileStorageSettings.DenyOverwrites || !overwrite))
                throw new FileOverwriteException(path);

            return path;
        }

        private DirectoryInfo CreateLocalDirectory(string fullPath)
        {
            if (!PathHelper.IsDirectory(fullPath))
                fullPath = PathHelper.GetDirectoryPath(fullPath);

            return Directory.CreateDirectory(fullPath);
        }

        public bool Exists(string path)
        {
            return File.Exists(ResolveFullPath(path));
        }

        public Task<bool> ExistsAsync(string path)
        {
            return Task.FromResult(Exists(path));
        }

        public Stream Load(string path)
        {
            return BuildFileStream(ResolveFullPath(path), async: false, write: false);
        }

        public Task<Stream> LoadAsync(string path)
        {
            var stream = (Stream)BuildFileStream(ResolveFullPath(path), async: true, write: false);
            return Task.FromResult(stream);
        }

        public string ReadContent(string path)
        {
            return File.ReadAllText(ResolveFullPath(path));
        }

        public async Task<string> ReadContentAsync(string path)
        {
            using (var reader = File.OpenText(ResolveFullPath(path)))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public void Delete(string path)
        {
            string fullPath = ResolveFullPath(path, canBeDirectory: true);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            else if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, recursive: true);
        }

        public Task DeleteAsync(string path)
        {
            Delete(path);
            return Task.CompletedTask;
        }

        public FileDirectory CreateDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(ResolveFullPath(path, canBeDirectory: true));
            directoryInfo = CreateLocalDirectory(directoryInfo.FullName);

            return new FileDirectory(path, directoryInfo);
        }

        public Task<FileDirectory> CreateDirectoryAsync(string path)
        {
            var directory = CreateDirectory(path);
            return Task.FromResult(directory);
        }

        public FileDirectory GetDirectory(string path, 
            FileDirectoryIncludeOption directoryIncludes = FileDirectoryIncludeOption.None,
            FilesIncludeOption fileIncludes = FilesIncludeOption.None)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            string fullPath = ResolveFullPath(path, canBeDirectory: true);
            var directoryInfo = new DirectoryInfo(fullPath);
            if (!directoryInfo.Exists)
                return null;

            var directory = new FileDirectory(path, directoryInfo);

            directory.LoadLocalChildren(directoryIncludes, fileIncludes, level: 1);

            return directory;
        }

        public Task<FileDirectory> GetDirectoryAsync(string path, 
            FileDirectoryIncludeOption directoryIncludes = FileDirectoryIncludeOption.None,
            FilesIncludeOption fileIncludes = FilesIncludeOption.None)
        {
            var directory = GetDirectory(path, directoryIncludes, fileIncludes);
            return Task.FromResult(directory);
        }

        public FileReference Save(Stream stream, string path, bool overwrite = false)
        {
            path = PreparePathForSave(path, overwrite);
            long size = 0;

            using (FileStream file = File.Create(path))
            {
                stream.CopyTo(file);
                size = file.Length;
            }

            return new FileReference(path, size);
        }

        public async Task<FileReference> SaveAsync(Stream stream, string path, bool overwrite = false)
        {
            path = PreparePathForSave(path, overwrite);
            long size = 0;

            using (FileStream fileStream = BuildFileStream(path, async: true, write: true))
            {
                await stream.CopyToAsync(fileStream);
                size = fileStream.Length;
            };

            return new FileReference(path, size);
        }

        public FileReference Save(string content, string path, bool overwrite = false)
        {
            path = PreparePathForSave(path, overwrite);

            File.WriteAllText(path, content);
            long size = new FileInfo(path).Length;

            return new FileReference(path, size);
        }

        public async Task<FileReference> SaveAsync(string content, string path, bool overwrite = false)
        {
            path = PreparePathForSave(path, overwrite);
            long size = 0;
            byte[] encodedText = Encoding.Unicode.GetBytes(content);

            using (FileStream fileStream = BuildFileStream(path, async: true, write: true))
            {
                await fileStream.WriteAsync(encodedText, 0, encodedText.Length);
                size = fileStream.Length;
            };

            return new FileReference(path, size);
        }
    }
}